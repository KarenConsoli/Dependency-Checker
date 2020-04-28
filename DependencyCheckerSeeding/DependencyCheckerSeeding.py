import os
import re
import sys, getopt
import json
import pyodbc

exts = [".hpp", ".cpp", ".c", ".h"]
pattern = "^#include <(.*?)>"
dbConnectionData = {
	"server": "localhost",
	"database": "DependencyChecker",
	"user": "root",
	"password": "root"
}
result = {}

# Read db connection file
configFilePath = "connectionstring.json"
if os.path.isfile(configFilePath):
	# Read
	with open(configFilePath, "r", encoding="utf-8") as file:
		configJson = json.loads(file.read())
		try:
			dbConnectionData["server"] = configJson["server"]
			dbConnectionData["database"] = configJson["database"]
			dbConnectionData["user"] = configJson["user"]
			dbConnectionData["password"] = configJson["password"]
		except KeyError as e:
			print(f"Property missing {e} in the database connection file")
			sys.exit(2)
		except Exception as e:
			print("Invalid database connection file:", str(e))
			sys.exit(2)
else:
	# Create file
	with open(configFilePath, "w", encoding="utf-8") as file:
		file.write(json.dumps(dbConnectionData))
	print("The database connection file was created. Edit it and run again.")
	sys.exit(2)

totalFiles = 0
totalDirs = 0
totalIncludes = 0

def processFile(path:str):
	global totalIncludes
	global totalFiles
	totalFiles += 1

	with open(path, encoding="utf-8") as file:
		data = file.read()
		result = re.findall(pattern, data, re.MULTILINE)
		totalIncludes += len(result)
		return result

def processFolder(path:str, mapFiles:map):
	global totalDirs
	global folder
	totalDirs += 1

	for file in os.listdir(path):
		if os.path.isdir(path+file):
			processFolder(path+file+"/", mapFiles)
		elif str(os.path.splitext(file)[1]) in exts:
			mapFiles[path.replace(folder, "").replace(file+"/", "")+file]=processFile(path+file)

# Comandos
folder = "code/"
try:
	opts, args = getopt.getopt(sys.argv[1:],"f:ve:v", ["folder=", "ext="])
	for o, v in opts:
		if o in ("-f", "--folder"):
			folder = (v if v[-1]=="/" else v+"/")
		elif o in ("-e", "--ext"):
			exts=["."+i for i in v.split(",")]
except getopt.GetoptError:
	print("args error")
	sys.exit(2)


processFolder(folder, result)

print(f"Processed Folders: {totalDirs}, Processed Files: {totalFiles} and Dependencies Found:  {totalIncludes}.")

with open("result.json", "w", encoding="utf-8") as file:
	file.write(json.dumps(result))
	print("result.json guardado!")

conn = pyodbc.connect('Driver={SQL Server};'
                      f'Server={dbConnectionData["server"]};'
                      f'Database={dbConnectionData["database"]};'
                      'Trusted_Connection=yes;'
                      f'uid={dbConnectionData["user"]};pwd={dbConnectionData["password"]};')
cursor = conn.cursor()

totalInserts = 0
for filePath in result:
	print("File ->", filePath)

	# Find FileId in db
	fileId = cursor.execute(f"SELECT FileId FROM [File] WHERE FileName='{filePath}'").fetchval()

	if fileId == None:
		# Insert File and get ID
		cursor.execute(f"INSERT INTO [File](FileName) VALUES ('{filePath}')")
		fileId = cursor.execute("SELECT @@IDENTITY AS ID").fetchval()
		totalInserts += 1
		#cursor.commit()

	# Find total FileDependency
	totalFileDependencies = cursor.execute(f"SELECT COUNT(*) FROM FileDependency WHERE FileId={fileId}").fetchval()

	if len(result[filePath]) > 0:
		# Hay dependencias
		for fileDependency in result[filePath]:
			print("Dependency ->", fileDependency)

			# Find DependencyId in db
			dependencyId = cursor.execute(f"SELECT DependencyId FROM Dependency WHERE DependencyName='{fileDependency}'").fetchval()

			if dependencyId == None:
				# Insert Dependency and get ID
				cursor.execute(f"INSERT INTO Dependency(DependencyName) VALUES ('{fileDependency}')")
				dependencyId = cursor.execute("SELECT @@IDENTITY AS ID").fetchval()
				totalInserts += 1
				#cursor.commit()

			# Verificar que no exista un registro identico
			fileDependencyId = cursor.execute(f"SELECT FileDependencyId FROM FileDependency WHERE FileId={fileId} AND DependencyId={dependencyId}").fetchval()
			if fileDependencyId == None:
				# No existe, insertar registro
				cursor.execute(f"INSERT INTO FileDependency(FileId, DependencyId) VALUES ({fileId}, {dependencyId})")
				totalInserts += 1
	elif totalFileDependencies == 0:
		# No hay ninguna dependencia, crear una nula
		cursor.execute(f"INSERT INTO FileDependency(FileId) VALUES ('{fileId}')")
		totalInserts += 1

cursor.commit()
print("-----------------------------------------------------------")
print("Dependencies successfully processed.")
print(f" Inserted Rows: {totalInserts}")
print("-----------------------------------------------------------")