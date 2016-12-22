import random
from os import urandom
import hashlib, binascii
random.seed(42)

# data generated with http://www.json-generator.com/
randomCities = ["Ladera","Vivian","Fowlerville","Chapin","Osmond","Harborton","Hendersonville","Wanship","Matthews","Brutus","Robinson","Martinez","Hollins","Dalton","Zortman","Lemoyne","Elbert","Crumpler","Salunga","Clarktown","Saddlebrooke","Newkirk","Stockwell","Brooktrails","Coldiron","Weogufka","Sabillasville","Cetronia","Brookfield","Greenbush","Kula","Beaulieu","Grazierville","Trona","Bascom","Fulford","Gerber","Trucksville","Yorklyn","Swartzville","Munjor","Somerset","Blanford","Loyalhanna","Villarreal","Hiwasse","Homeworth","Websterville","Starks","Indio","Johnsonburg","Belvoir","Cornfields","Geyserville","Elizaville","Tetherow","Fairmount","Bluetown","Dorneyville","Layhill","Eden","Welch","Rew","Weeksville","Driftwood","Bonanza","Galesville","Glenshaw","Ventress","Smock","Canoochee","Conway","Hebron","Austinburg","Sunbury","Cliff","Waterloo","Orviston","Richford","Wiscon","Madaket","Aberdeen","Cannondale","Outlook","Dodge","Riegelwood","Sparkill","Bridgetown","Muir","Kansas","Hardyville","Motley","Germanton","Gratton","Independence","Trinway","Saticoy","Onton","National","Shindler","Yogaville","Loretto","Elliott","Ezel","Castleton","Leyner","Cavalero","Morriston","Denio","Unionville","Norfolk","Dundee","Joppa","Waterford","Alamo","Greenwich","Thomasville","Joes","Baden","Tooleville","Marne","Lund","Disautel","Frizzleburg","Cazadero","Williston","Oretta","Boling","Grandview","Titanic","Rosine","Stewart","Fresno","Deseret","Camptown","Taft","Bennett","Shrewsbury","Wheatfields","Linwood","Blandburg","Martell","Woodruff","Bowmansville","Florence","Allensworth","Reinerton","Westmoreland","Mayfair","Lutsen","Snelling","Draper","Blue","Linganore","Olney","Dotsero","Whipholt","Herbster","Goldfield","Grantville","Kimmell","Staples","Gwynn","Hailesboro","Kohatk","Genoa","Fairfield","Bedias","Dexter","Brecon","Nicut","Glenville","Ilchester","Dunlo","Holtville","Lopezo","Jennings","Libertytown","Fairacres","Chesterfield","Lindcove","Delshire","Colton","Kiskimere","Abiquiu","Mooresburg","Freelandville","Tryon","Cresaptown","Chilton","Maxville","Cliffside","Belva","Bakersville","Valmy","Sunnyside","Wildwood","Turpin","Vallonia","Hamilton","Witmer","Southview","Imperial","Allison","Sunwest","Bentley","Whitmer","Adamstown","Shelby","Eggertsville","Sterling","Hayes","Farmington","Ferney","Matheny","Terlingua","Fedora","Evergreen","Hemlock","Mammoth","Snowville","Rodanthe","Umapine","Veyo","Calpine","Dixonville","Kent","Springville","Idamay","Coaldale","Hilltop","Hachita","Emory","Enetai","Cecilia","Noblestown","Roderfield","Maury","Enlow","Dargan","Crayne","Bluffview","Logan","Barrelville","Adelino","Wheaton","Frystown","Rockingham","Charco","Tonopah","Dowling","Fontanelle","Laurelton","Kenmar","Moquino","Zeba","Cade","Kipp","Herlong","Soham","Veguita","Marenisco","Torboy","Holcombe","Bowie","Reno","Thatcher","Chalfant","Goochland","Rutherford","Basye","Heil","Coventry","Wyano","Sedley","Elrama","Volta","Brogan","Downsville","Lupton","Eastmont","Cotopaxi","Eureka","Graniteville","Jacumba","Dola","Albrightsville","Hatteras","Shawmut","Innsbrook","Cherokee","Datil","Dragoon","Naomi","Groveville","Hobucken","Sanford","Boonville","Ripley","Greenock","Maybell","Dennard","Cobbtown","Summerset","Longoria","Ticonderoga","Mappsville","Shaft","Dunnavant","Snyderville","Lewis","Northridge","Cascades","Healy","Savannah","Konterra","Kerby","Nash","Slovan","Lafferty","Davenport","Crown","Gilmore","Carbonville","Fairhaven","Catherine","Efland","Waterview","Dahlen","Odessa","Chemung","Wright","Cuylerville","Sidman","Harmon","Whitestone","Bodega","Bartonsville","Wedgewood","Rose","Balm","Forbestown","Stagecoach","Rossmore","Ruffin","Leming","Mapletown","Orick","Cawood","Spelter","Dawn","Alden","Rushford","Floriston","Harold","Calverton","Glidden","Grahamtown","Mulino","Kapowsin","Ebro","Frierson","Leland","Stockdale","Bend","Welda","Beaverdale","Nord","Accoville","Stouchsburg","Worton","Freeburn","Lynn","Loveland","Thynedale","Warren","Hillsboro","Kraemer","Neibert","Englevale","Tibbie","Iberia","Croom","Helen","Rowe","Beason","Twilight","Winston","Albany","Caroleen","Harviell","Nicholson","Omar","Freetown","Delwood","Osage","Craig","Golconda","Sperryville","Chaparrito","Glasgow","Lacomb","Keyport","Soudan","Jacksonburg","Axis","Chicopee","Hampstead","Summerfield","Nadine","Eagletown","Shepardsville","Singer","Lodoga","Mansfield","Machias","Jacksonwald","Ogema","Rockbridge","Tilden","Rodman","Magnolia","Curtice","Como","Homeland","Kingstowne","Barronett","Ypsilanti","Inkerman","Spokane","Windsor","Caledonia","Stewartville","Boomer","Tuskahoma","Tolu","Harrodsburg","Nelson","Drummond","Sehili","Floris","Romeville","Kilbourne","Kidder","Lloyd","Haring","Longbranch","Hasty","Westwood","Watrous","Oceola","Bison","Zarephath","Kylertown","Vowinckel","Diaperville","Norwood","Nettie","Cutter","Hondah","Chumuckla","Felt","Ironton","Sugartown","Trail","Ivanhoe","Loomis","Gilgo","Wakulla","Harleigh","Clara","Foscoe","Bainbridge","Cashtown","Esmont","Falmouth","Aurora","Hessville","Forestburg","Guilford","Iola","Chelsea","Faywood","Sisquoc","Elwood","Delco","Kersey","Cochranville","Ballico","Rivers","Idledale","Allendale","Southmont","Statenville","Winfred","Ruckersville","Alleghenyville","Navarre","Coyote","Rote","Boykin","Chautauqua","Finderne","Gorst","Highland","Brandywine","Tivoli","Whitehaven","Brantleyville","Thornport","Sunriver","Caln","Franklin","Hartsville/Hartley","Foxworth","Drytown","Epworth","Baker","Cleary","Berwind","Lindisfarne","Churchill","Kaka","Topanga","Brenton","Oberlin","Marion","Dana","Choctaw","Carrsville","Edinburg","Gambrills","Teasdale","Bourg","Kirk","Nutrioso","Westerville","Edenburg","Glenbrook","Edgar","Echo","Trexlertown","Jamestown","Oasis","Utting","Gordon","Garnet","Canby","Watchtower","Homestead","Corriganville","Sandston","Nescatunga","Brethren","Lumberton","Strong","Selma","Goodville","Montura","Rosedale","Graball","Smeltertown","Gibsonia","Marysville","Crenshaw","Brandermill","Guthrie","Defiance","Bagtown","Yukon","Jenkinsville","Riviera","Ada","Norris","Jeff","Bergoo","Enoree","Loma","Rockhill","Jardine","Bartley","Brewster","Coalmont","Hayden","Malo","Chase","Marienthal","Cumminsville","Harrison","Bendon","Marbury","Devon","Collins","Coultervillle","Fruitdale","Conestoga","Sims","Newcastle","Sharon","Dale"]
randomNames = ["Camacho","Ayala","Opal","Kelli","Fry","Klein","Janie","Lou","Barrera","Dixon","Francis","Dyer","Valenzuela","Langley","Carla","Karin","Marguerite","Lakisha","Porter","Lilly","Watkins","Selma","Felecia","Mayra","Koch","Gamble","Rosalie","Guthrie","Estelle","Chandra","Matthews","Nora","Toni","Morse","Rachelle","Bridgett","Bianca","Trisha","Carter","Becky","Rose","Lillie","Taylor","Frankie","Carmella","Blake","Harrell","Martinez","Bethany","Livingston","Tamika","Olga","Kidd","Gross","Mills","Decker","Cervantes","Melissa","Osborne","Audrey","Donaldson","Taylor","Quinn","Bowers","Marcie","Margie","Howe","Anderson","Tameka","Barron","Mary","Lula","Margarita","Colon","Small","Sykes","Dina","Hudson","Ruthie","Erna","Irwin","Hodges","Bennett","Louisa","Jeannette","Steele","Calhoun","Mcintyre","Lois","Katie","Olivia","Washington","Amanda","Jeannie","Elvira","Lambert","Deanne","Sherman","Barrett","Cheryl","Mcdowell","Tina","Reyes","Mckinney","Huber","Harriet","Graves","Ester","Bowman","Vazquez","Maryellen","Burch","Whitehead","Pearlie","Lucia","Sampson","Maude","Hernandez","Roslyn","Terry","Rowe","Hoffman","Dominguez","Delacruz","Albert","Osborn","Willa","Jeanette","Levy","Trevino","Alta","Massey","Hannah","Keller","Ida","Jamie","Marie","Combs","Jacklyn","Mayo","Kendra","Mcdaniel","Brittney","Kirby","Nguyen","Lynnette","Mildred","Joyce","Wendy","Valdez","Jimmie","Barton","Kara","Latoya","Jami","Cole","Adriana","Benita","Fannie","Floyd","Johns","Stein","Patti","Diann","Jewel","Mercedes","Prince","Nieves","Joanne","Lara","Nixon","Angel","Renee","Eddie","Dolores","Cummings","Frances","Howell","Robinson","Summers","Hull","Nelson","Abbott","Stone","Compton","Deloris","Zelma","Cristina","Kristen","Elba","Magdalena","Rosalyn","Liza","Holland","Angelia","Rochelle","Pena","Chapman","Jenny","Townsend","Lucinda","Peterson","Jill","Claudette","Ramona","Ayers","Sandoval","Christian","Parrish","Lawson","Alvarez","Sharlene","Mcclure","Annie","May","Munoz","Teresa","Sonya","Nell","Robin","Montoya","Milagros","Gomez","Lowe","Bell","Virginia","Pittman","Marietta","Hampton","Jordan","Frazier","Edith","Hurley","Daniels","Myrna","Ella","Annette","Carolina","Virgie","Maricela","Irma","Evans","Tracie","Nannie","Head","Logan","Weeks","Coleen","Maddox","Short","Beasley","Judy","Franklin","Park","Page","Stanton","Eleanor","Heather","Schneider","Violet","Patterson","Jacqueline","Sharron","Casandra","Marks","Parks","Daniel","Karen","Foley","Josephine","Lea","Bowen","Rosalinda","Meadows","Alberta","Parsons","Corine","Celia","Spencer","Randi","Vincent","Angelica","Deleon","Marquita","Ernestine","Burnett","Cleveland","Randall","Vaughn","Bradford","Antonia","Nicholson","Patrice","Boyle","Yvette","Sloan","Kitty","Ellis","Sylvia","Sherri","Sears","Caitlin","Patrick","Candy","Trina","Aimee","Vicki","Glenn","Tabitha","Bray","Wolfe","Debora","Acosta","Sabrina","Rocha","Leah","Jennings","Daugherty","Jessie","Romero","Keri","Lenore","Kimberley","Turner","Sophia","Leon","Ann","Leslie","Sargent","Mccarty","Fay","Shawna","Leanna","Katina","Glenna","Meagan","Carolyn","Lela","Avila","Richards","Rosa","Williams","Hines","Susana","Neva","Simpson","Lola","Cortez","Blankenship","Wall","Gibbs","Whitley","Herman","Norma","Gretchen","Vinson","Mays","Clarissa","Bette","Barr","Scott","Hopkins","Bridgette","Moody","Richardson","Bruce","Tammi","Hendrix","Barber","Cora","Burris","Herring","Patrica","Rich","Wilkins","Hopper","Miller","Joyner","Strickland","Debbie","Adrian","Iris","Corrine","Maureen","Stevens","Shauna","Tasha","Byers","Kinney","Rosalind","Mosley","Lee","Leila","Heidi","Fitzgerald","Nadia","Lott","Celina","Shaw","Lorna","Colleen","Ingrid","Moon","Clara","Calderon","Jenifer","Dillon","Lorrie","Skinner","Powers","Briggs","Kathrine","Harrison","Janine","Lucas","Pratt","Oneill","Underwood","Morton","Ware","Carson","Wolf","Naomi","Nettie","Franco","Patton","Bessie","Carmen","Lorie","Erica","Freida","Cobb","Lenora","Viola","Amalia","Key","Fox","Hatfield","Brandie","Ursula","Barbara","Rachael","Imelda","Leigh","Jacquelyn","Felicia","Roxanne","Deidre","Brigitte","Nichols","Lidia","Ferrell","Schwartz","Jodie","Simon","Golden","Coffey","Waller","Augusta","Esmeralda","Mckay","Tessa","Helena","Gabrielle","Julia","Fitzpatrick","Lindsay","Hancock","Manuela","Paige","Murphy","Eliza","Banks","Estela","Day","Etta","Glover","Alexandra","Emilia","Eva","Kramer","Boyd","Juanita","Petty","Sullivan","Rosales","Hogan","Annmarie","Claire","Wiley","Gwendolyn","Campos","Nikki","Bauer","Ronda","Thomas","Guadalupe","Hester","Workman","Doyle","Minnie","Eileen","Allison","Lina","England","Cline","Peters","Woodard","Adkins","Lorene","Walsh","French","Katelyn","Jeri","Laurie","Sheryl","Mitchell","Maryanne","Winifred","Freda","Cecilia","Gena","Berger","Nita","Lavonne","Ray","Erin","Landry","Houston","Jannie","Foster","Dona","Suzanne","Julie","Sawyer","Fuller","Concetta","Velazquez","Marissa","Noemi","Mullen","Twila","Martha","Gina","Fernandez","Mack","English","Foreman","Hess","Becker","Reba","Sparks","Hood","Bonner","Sonia","Young","Nona","Hubbard","Cassie","Lacey","Janell","Dee","Carmela","Drake","Camille","Ericka","Keith","Christine","Evangeline","Johnson","Gay","Jaime","Mcknight","Joy","Tricia","Knox","Bean","Michael","Reva","Mcneil","Ross","Gardner","Rowena","Reeves","Kathy","Hoover","Webb","Kaitlin","Noble","Stout","Stacy","Aguilar","Hooper","Marlene","Lolita","Ashley","Christa","Jordan","Sheppard","Peggy"]
randomPlayer = [{"firstName":"Herwig","lastName":"Maurer","nickName":"Herbi","userName":"mauerpower","password":"topsecret","isAdmin":1},{"firstName":"Stephanie","lastName":"Vierseitl","nickName":"Steffi","userName":"viererverlierer","password":"3.14159265359","isAdmin":1},{"firstName":"Stefan","lastName":"Vordermetaller","nickName":"VoMe","userName":"Hiho","password":"BWLRWEGPM","isAdmin":0},{"firstName":"Birgit","lastName":"Hai","nickName":"der Hai","userName":"achtungbissig","password":"droptablestudents","isAdmin":1},{"firstName":"Markus","lastName":"Spinner","nickName":"Springi","userName":"fcbayern","password":"bayernoleeee","isAdmin":0},{"firstName":"Andrea","lastName":"Hobelmayer","nickName":"Hobli","userName":"hobeldobel","password":"KatzeHundMaus","isAdmin":0},{"firstName":"Connie","lastName":"Cole","nickName":"reprehenderit","userName":"mollit","password":"laboris","isAdmin":0},{"firstName":"Sheena","lastName":"Schneider","nickName":"cupidatat","userName":"aliquip","password":"ut","isAdmin":0},{"firstName":"Tanner","lastName":"Hancock","nickName":"laborum","userName":"enim","password":"aliqua","isAdmin":0},{"firstName":"Oliver","lastName":"Miranda","nickName":"tempor","userName":"qui","password":"fugiat","isAdmin":0},{"firstName":"Pauline","lastName":"Terry","nickName":"veniam","userName":"non","password":"dolor","isAdmin":0},{"firstName":"Blackburn","lastName":"Hoffman","nickName":"consequat","userName":"ea","password":"non","isAdmin":0},{"firstName":"Ross","lastName":"Henson","nickName":"aliqua","userName":"ex","password":"deserunt","isAdmin":0},{"firstName":"Maura","lastName":"Sweet","nickName":"qui","userName":"culpa","password":"sit","isAdmin":0},{"firstName":"Geneva","lastName":"Morin","nickName":"anim","userName":"Lorem","password":"fugiat","isAdmin":0},{"firstName":"Hilary","lastName":"Boyd","nickName":"eiusmod","userName":"amet","password":"tempor","isAdmin":0},{"firstName":"Jewel","lastName":"Dennis","nickName":"enim","userName":"cillum","password":"occaecat","isAdmin":0},{"firstName":"Celina","lastName":"Cote","nickName":"ut","userName":"veniam","password":"pariatur","isAdmin":0},{"firstName":"Warner","lastName":"Summers","nickName":"laborum","userName":"consectetur","password":"enim","isAdmin":0},{"firstName":"Powers","lastName":"Smith","nickName":"culpa","userName":"anim","password":"officia","isAdmin":0},{"firstName":"Bernard","lastName":"Cummings","nickName":"voluptate","userName":"voluptate","password":"laborum","isAdmin":0},{"firstName":"Boone","lastName":"Coleman","nickName":"elit","userName":"tempor","password":"exercitation","isAdmin":0},{"firstName":"Rojas","lastName":"Burks","nickName":"proident","userName":"occaecat","password":"fugiat","isAdmin":0},{"firstName":"Taylor","lastName":"Stokes","nickName":"commodo","userName":"est","password":"irure","isAdmin":0},{"firstName":"Millicent","lastName":"Spence","nickName":"ullamco","userName":"do","password":"aliquip","isAdmin":0},{"firstName":"Kirkland","lastName":"Chapman","nickName":"mollit","userName":"exercitation","password":"eiusmod","isAdmin":0},{"firstName":"Aguirre","lastName":"Snyder","nickName":"sunt","userName":"laborum","password":"id","isAdmin":0},{"firstName":"Dickson","lastName":"Sawyer","nickName":"incididunt","userName":"voluptate","password":"ex","isAdmin":0},{"firstName":"Camacho","lastName":"Michael","nickName":"dolor","userName":"ex","password":"laboris","isAdmin":0},{"firstName":"Angelita","lastName":"Morales","nickName":"reprehenderit","userName":"in","password":"occaecat","isAdmin":0}]


def insertInto(table, keys, values):
	statement = "INSERT INTO [dbo].[" + table + "] ("
	
	statement += "[" + keys[0] + "]"
	for key in keys[1:]:
		statement += ",[" + key + "]"
	
	statement += ") VALUES ("
	statement += str(values[0])
		
	for value in values[1:]:
		statement += "," + str(value)
		
	statement += ");\n"
	return statement
	
def quote(str):
	return "'" + str + "'"
   
randomDataKeys = ["firstName","lastName", "nickName", "userName","password","isAdmin"]
dayKeys = ["playsMondays", "playsTuesdays", "playsWednesdays", "playsThursdays", "playsFridays", "playsSaturdays", "playsSundays"]
allKeys = ["firstName","lastName", "nickName", "userName","password","salt","isAdmin"] + dayKeys


with open("dbo.Testdata.sql", "w") as f: # Player
	i = 0
	for player in randomPlayer[:25]:
		values = []
		if i > 5:
			player["userName"] = "player" + str(i) # make sure our username is unique
		i += 1
		for key in randomDataKeys[:-2]:
			values.append(quote(player[key])) # all from randomDataKeys but isAdmin
		
		salt = urandom(32)
		password = hashlib.pbkdf2_hmac('sha256', randomDataKeys[-2], salt, 100000)
		values.append("0x" + binascii.hexlify(salt)) 
		values.append("0x" + binascii.hexlify(password))
		
		values.append(player[randomDataKeys[-1]])
		
		for _ in dayKeys:
			isPlaying = int(random.random() < 0.4)
			values.append(isPlaying)
		f.write(insertInto("Player", allKeys, values))

    # Tournament
	for i in range(365*2):
		k = random.randint(0, len(randomNames) - 1)
		l = random.randint(0, len(randomNames) - 1)
		admin = random.randint(0, 1)
		f.write(insertInto("Tournament", ["name", "datetime"], [quote(randomCities[k]+"_"+randomNames[l]), "dateadd(day, " + str(i) + ", cast('2014-01-02 07:36:13.000' as datetime2))"]))
	
	# Rating
	for playerid in range(30):
		rating = 2000
		for i in range(365*2):
			f.write(insertInto("Rating", ["playerId", "datetime", "value"], [playerid, "dateadd(day, " + str(i) + ", cast('2014-01-02 07:36:13.000' as datetime2))", rating]))
			rating += random.randint(-50, 50)
			if rating < 1: 
				rating = 1
			
			


	# Match
	STARTID = 0 # first tournamentId 
	
	for i in range(365*2): # 2 years
		amount = random.randint(0, 8) # between 0 and 8 matches each day => avg. 4
		
		for j in range(amount):
			winner = random.random()
			if winner < 0.5:	
				score1 = 10
				score2 = random.randint(0, 9)
			else:
				score1 = random.randint(0, 9)
				score2 = 10
				
			Ea = random.random() # estimated probability of team 1 winning, here random
			
			p1, p2, p3, p4 = random.sample(range(30), 4)
			
			f.write(insertInto("Match", 
				["tournamentId", "datetime", "player1", "player2", "player3", "player4", "scoreTeam1", "scoreTeam2", "EstimatedWinChance", "isDone"],
				[STARTID + i, "dateadd(day, " + str(i) + ", '2014-01-02 07:36:13.000')", p1, p2, p3, p4, score1, score2, Ea, 1]))
				
	f.write(insertInto("ScoreParameter", ["key", "value"], [quote("initialscore"), quote("2000")]))
	f.write(insertInto("ScoreParameter", ["key", "value"], [quote("scoredMatches"), quote("100")]))
	f.write(insertInto("ScoreParameter", ["key", "value"], [quote("halflife"), quote("50")]))
	f.write(insertInto("ScoreParameter", ["key", "value"], [quote("k-rating"), quote("32")]))
	f.write(insertInto("ScoreParameter", ["key", "value"], [quote("timepenalty"), quote("50")]))

