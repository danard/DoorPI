const express = require('express')
const app = express()
const fileUpload = require('express-fileupload');
const { exec } = require("child_process");
const port = 8080

var fs = require('fs');
var url = require('url');
var path = require('path');
var mqtt = require('mqtt');
var client = mqtt.connect('mqtt://172.16.4.33:8081');

app.use(express.json());
app.use(fileUpload());


var usersDB;
var testMedia;


client.on('connect', function () { //MQTT connected to the broker
    console.log("mqtt connected");
})


function checkCredentials(user, pw) {
    var i;
    for (i = 0; i < usersDB.users.length; i++){
        if (usersDB.users[i].user == user && usersDB.users[i].password == pw){
            return 1;
        }
    }
    return 0;
}

function getUserFromIP(ip){
	var i;
	for (i = 0; i < usersDB.users.length; i++){
		if(usersDB.users[i].ip == ip){
			return usersDB.users[i].user;
		}
	}
	return 'none';
}

function getPW(user){
	var i = 0;
	while (i < usersDB.users.length){
		if(usersDB.users[i].user == user){
			return usersDB.users[i].password;
		}
	}
	return null;
}

app.get('/', (req, res) => {
  	res.send('PTI - DoorPI Webserver API')
})

app.get('/testpush', (req, res) => {
	//let dir = `${__dirname}/../../../imgtest.png`;
	let dir;
	if(testmedia == 0) dir = `${__dirname}/media/test/img1.jpeg`;
	else dir = `${__dirname}/media/test/img2.jpeg`;

	testmedia = (testmedia + 1) % 2;

	fs.readFile(dir, "base64", (err, data) => { //Send image as a push notification using MQTT
        if(err) {
            console.log(err.code);
        }

        var img64 = data;
        var buf = Buffer.from(data, "base64");

		var user = "sara";
		let pw = getPW(user);
		//console.log("publishing to: " + `access-images-${user}${pw}`);
        client.publish(`access-images-${user}${pw}`, buf);
    })

	res.send("Sent image upload request to the server, a notification should be on its way")
})


//Register the Raspberry in the server or update its information
//
//A username, a password and a streaming link must be provided.
//
//If the username is already in use (Raspberry was previously used),
//The information is updated. This includes password, link to stream and
//the Raspberry's IP Address.
//If not, a new Raspberry is registered.
app.post('/regrb', (req, res, next) => { //Register the Raspberry
	let raspUser = req.body.usuario;
	let raspPW = req.body.contrasena;
	let raspStream = req.body.stream;
	let raspIP = req.ip;

	if ((typeof raspUser == "undefined") || (typeof raspPW == "undefined") || (typeof raspStream == "undefined")){
		console.log("undefined parameter(s) in raspberry registration");
		res.status(400);
		res.end();
	}


	obj = usersDB;

	var found = 0;
    var i;
    for (i = 0; i < obj.users.length; i++){
        if (obj.users[i].user == raspUser){ //If the rasp is already in use, update data
            obj.users[i].password = raspPW;
            obj.users[i].stream = raspStream;
			obj.users[i].ip = raspIP;
            found = 1;
        }
    }
	let newUser;
    if (!found){ //User not found, add to database
        newUser = {
            user: raspUser,
            password: raspPW,
            stream: raspStream,
			ip: raspIP
        }
        obj.users.push(newUser); //Add new user

		exec("mkdir ./media/"+raspUser, (err) => { //Create user media directory
			if (err) console.log("Error creating user directory: " + err.code);
		})
    }

    let newData = JSON.stringify(obj);
    fs.writeFileSync(`${__dirname}/db/users.json`,newData, function(err) { //Write to Database
		if (err){
			console.log("Error writing userDB: " + newUser);
		}
	})

	usersDB = obj;

	res.status(200);
	res.end();
})

//Get the stream URL
//
//A user and password are needed for authentication
app.post('/stream', (req, res, next) => {
	let user = req.body.usuario;
	let pw = req.body.contrasena;

	var found = 0;
	var i;
	for (i = 0; i < usersDB.users.length; i++){
		if (usersDB.users[i].user == user && usersDB.users[i].password == pw){
			res.send(usersDB.users[i].stream);
			found = 1;
			break;
		}
	}
	if (found == 0){
		res.status(401);
	}
	else res.status(200);
	res.end();
})

//Get a video from the server
//
//A username and a password are needed for authentication
//A videoID are needed to request a specific video
app.post('/vid/', (req, res, next) => {
	let user = req.body.usuario;
	let pw = req.body.contrasena;

	if(checkCredentials(user,pw) == 0){
		res.status(401);
		res.end();
		return;
	}

	let dir = `${__dirname}/media/${user}/${req.body.videoID}`;

	fs.access(dir, (err) => {
		if(!err) return;
		console.log("Error loading video. Error : "+err.code);
		res.status(404);
		res.end();
		return;
	})

	res.sendFile(dir, function (err) {
		if (err){
			console.log("Video failed to send: " + err.code + ". Video path: " + dir);
			res.status(500);
		}
		res.end();
	})
})

//Request a specific image form the server
//
//A username and a password are needed for authentication
//The image of the name must be provided
app.post('/img', (req, res, next) => {
    let user = req.body.usuario;
    let pw = req.body.contrasena;

    if(checkCredentials(user,pw) == 0){
        res.status(401);
        res.end();
        //return;
    }

	let dir = `${__dirname}/media/${user}/${req.body.img}`;

    fs.access(dir, (err) => {
        if(!err) return;
        console.log("Error loading video. Error : "+err.code);
        res.status(404);
        res.end();
        return;
    })

    res.sendFile(dir, function (err) {
        if (err){
            console.log("Video failed to send: " + err.code + ". Video path: " + dir);
            res.status(500);
        }
        res.end();
	})
})

//Upload a video to the server
//
//The Raspberry's IP Address is used for authentication
app.post('/uploadvid', (req, res, next) => {
	var ip = req.ip;

    var user = getUserFromIP(ip);
    if(user == 'none'){
        console.log(`Error: received upload request with ip ${ip}, but user was not found.`);
        res.status(401);
        res.end();
        return;
    }

	if (!req.files || Object.keys(req.files).length === 0) {
    	return res.status(400).send('No files were uploaded.');
  	}


	let video = req.files.video;
	let dir = `${__dirname}/media/${user}/${video.name}`;
	video.mv(dir, function(err) { //Save video to user's media folder
		if (err){
			console.log("Error uploading video: " + err.code);
			res.status(500);
			res.end();
		}
	//Add video to user video list
	})
	res.end();
})

//Upload an image to the server and send a push notification to the app
//
//The Raspberry's IP Address is used for authentication
app.post('/uploadimg', (req, res, next) => {
	var ip = req.ip;

	var user = getUserFromIP(ip);
	var pw = getPW(user);

	//IP desconocida
	if(user == 'none'){
		console.log(`Error: received upload request with ip ${ip}, but user was not found.`);
		res.status(401);
		res.end();
		return;
	}

	//Control de errores: petición sin archivos o tamaño nulo
	if(!req.files || Object.keys(req.files).length === 0) {
		return res.status(400).send('No files were uploaded');
	}

	console.log(`Uploading image with IP ${ip} for user ${user}`);

	let img = req.files.img;
	let dir = `${__dirname}/media/${user}/${img.name}`;
	img.mv(dir, function(err) { //Save image to user's media folder
		if(err){
			console.log("Error uploading image: " + err.code);
			res.status(500);
			res.end();
			return;
		}
	})

	fs.readFile(dir, "base64", (err, data) => { //Send image as a push notification using MQTT
		if(err) {
			console.log("Unable to read image: " + err.code);
		}

		var img64 = data;
		var buf = Buffer.from(img64, "base64");

    	client.publish(`access-images-${user}${pw}`, buf);
	})

	res.status(200);
	res.end();
})

//Create server, load user database
app.listen(port, () => {
	testmedia = 0;
	fs.readFile(`${__dirname}/db/users.json`, (err, data) => { //Load user database
		if (err){
			console.log("Error loading user database: " + err);
		}
		console.log("User DB loadaed");
		usersDB = JSON.parse(data);
	});

	console.log('HTTP Server listening')
})

//nattech.fib.upc.edu:40330
