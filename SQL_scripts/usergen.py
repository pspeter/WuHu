import random

# data generated with http://www.json-generator.com/
data = [{"firstName":"Summer","lastName":"Ewing","nickName":"sit","userName":"sit","password":"eiusmod","isAdmin":0},{"firstName":"Connie","lastName":"Cole","nickName":"reprehenderit","userName":"mollit","password":"laboris","isAdmin":0},{"firstName":"Sheena","lastName":"Schneider","nickName":"cupidatat","userName":"aliquip","password":"ut","isAdmin":0},{"firstName":"Tanner","lastName":"Hancock","nickName":"laborum","userName":"enim","password":"aliqua","isAdmin":0},{"firstName":"Oliver","lastName":"Miranda","nickName":"tempor","userName":"qui","password":"fugiat","isAdmin":0},{"firstName":"Pauline","lastName":"Terry","nickName":"veniam","userName":"non","password":"dolor","isAdmin":0},{"firstName":"Blackburn","lastName":"Hoffman","nickName":"consequat","userName":"ea","password":"non","isAdmin":0},{"firstName":"Ross","lastName":"Henson","nickName":"aliqua","userName":"ex","password":"deserunt","isAdmin":0},{"firstName":"Maura","lastName":"Sweet","nickName":"qui","userName":"culpa","password":"sit","isAdmin":0},{"firstName":"Geneva","lastName":"Morin","nickName":"anim","userName":"Lorem","password":"fugiat","isAdmin":0},{"firstName":"Hilary","lastName":"Boyd","nickName":"eiusmod","userName":"amet","password":"tempor","isAdmin":0},{"firstName":"Jewel","lastName":"Dennis","nickName":"enim","userName":"cillum","password":"occaecat","isAdmin":0},{"firstName":"Celina","lastName":"Cote","nickName":"ut","userName":"veniam","password":"pariatur","isAdmin":0},{"firstName":"Warner","lastName":"Summers","nickName":"laborum","userName":"consectetur","password":"enim","isAdmin":0},{"firstName":"Powers","lastName":"Smith","nickName":"culpa","userName":"anim","password":"officia","isAdmin":0},{"firstName":"Bernard","lastName":"Cummings","nickName":"voluptate","userName":"voluptate","password":"laborum","isAdmin":0},{"firstName":"Boone","lastName":"Coleman","nickName":"elit","userName":"tempor","password":"exercitation","isAdmin":0},{"firstName":"Rojas","lastName":"Burks","nickName":"proident","userName":"occaecat","password":"fugiat","isAdmin":0},{"firstName":"Taylor","lastName":"Stokes","nickName":"commodo","userName":"est","password":"irure","isAdmin":0},{"firstName":"Millicent","lastName":"Spence","nickName":"ullamco","userName":"do","password":"aliquip","isAdmin":0},{"firstName":"Kirkland","lastName":"Chapman","nickName":"mollit","userName":"exercitation","password":"eiusmod","isAdmin":0},{"firstName":"Aguirre","lastName":"Snyder","nickName":"sunt","userName":"laborum","password":"id","isAdmin":0},{"firstName":"Dickson","lastName":"Sawyer","nickName":"incididunt","userName":"voluptate","password":"ex","isAdmin":0},{"firstName":"Camacho","lastName":"Michael","nickName":"dolor","userName":"ex","password":"laboris","isAdmin":0},{"firstName":"Angelita","lastName":"Morales","nickName":"reprehenderit","userName":"in","password":"occaecat","isAdmin":0}]

def insertInto(table, keys, values):
    statement = "INSERT INTO [dbo].[" + table + "] ("
    
    statement += "[" + keys[0] + "]"
    for key in keys[1:]:
        statement += ",[" + key + "]"
    
    statement += ") VALUES ("
    
    if (type(values[0]) is str):
        statement += "'" + values[0] + "'"
    else:
        statement += str(values[0])
        
    for value in values[1:]:
        if (type(value) is str):
            statement += ",'" + value + "'"
        else:
            statement += "," + str(value)
        
    statement += ")"
    return statement
    
keys = ["firstName","lastName", "nickName", "userName","password","isAdmin"]

# Person
for person in data:
    values = []
    for key in keys:
        values.append(person[key])
    print(insertInto("Player", keys, values))

print
print

# Plays_On
days = ["Montag", "Dienstag", 'Mittwoch', 'Donnerstag', 'Freitag']
for i in range(30):
    for day in days:
        if 0.2 < random.random():
            print(insertInto("Plays_on", ["playerId", "Day"], [i, day]))
            
            
# Ranking
for i in range(30):
    print(insertInto("Rating", ["playerId", "value"], [i, 2000]))