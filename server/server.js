const express = require('express')
const app = express()
const port = 8080
const hls = require('hls-server');
const fileUpload = require('express-fileupload');
const { exec } = require("child_process");

var fs = require('fs');
var filepath = './media/';
var url = require('url');
var path = require('path');

app.use(express.json());
app.use(fileUpload());


var usersDB;


function checkCredentials(user, pw) {
	var found = 0;
    var i;
    for (i = 0; i < usersDB.users.length; i++){
        if (usersDB.users[i].user == user && usersDB.users[i].password == pw){
            res.json({stream: usersDB.users[i].stream});
            found = 1;
            break;
        }
    }
    if (found == 0){
        return 0;
    }
    else return 1;
}

app.get('/', (req, res) => {
  	res.send('Running - PTI - DoorPI')
	//return res.status(200).sendFile(`${__dirname}/client.html`);
})

app.post('/regrb', (req, res, next) => { //Register the Raspberry
	let raspUser = req.body.usuario;
	let raspPW = req.body.contrasena;
	let raspStream = req.body.stream;

	obj = usersDB;

	var found = 0;
    var i;
    for (i = 0; i < obj.users.length; i++){
        if (obj.users[i].user == raspUser){ //If the rasp is already in use, update data
            obj.users[i].password = raspPW;
            obj.users[i].stream = raspStream;
            found = 1;
        }
    }
	let newUser;
    if (!found){ //User not found, add to database
        newUser = {
            user: raspUser,
            password: raspPW,
            stream: raspStream
        }
        obj.users.push(newUser); //Add new user

		exec("mkdir ./media/"+raspUser, (err) => {
			if (err) console.log("Error creating user directory: " + err.code);
		})
    }

    let newData = JSON.stringify(obj);
    fs.writeFileSync(`${__dirname}/db/users.json`,newData, function(err) {
		if (err){
			console.log("Error writing userDB: " + newUser);
		}
	})

	usersDB = obj;

	res.status(200);
	res.end();
})

app.post('/stream', (req, res, next) => { //Get the stream URL
	let user = req.body.usuario;
	let pw = req.body.contrasena;

	console.log(req.body);
	console.log("request with: " + user + ", " + pw);

	var found = 0;
	var i;
	for (i = 0; i < usersDB.users.length; i++){
		if (usersDB.users[i].user == user && usersDB.users[i].password == pw){
			//res.json({stream: usersDB.users[i].stream});
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


app.post('/req', (req, res, next) => {

})

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


app.post('/upload', (req, res, next) => {
	var dt = JSON.parse(req.body);

	let user = dt.usuario;
	let pw = dt.contrasena;

	console.log(dt);

	if(checkCredentials(user,pw) == 0){
		res.status(401);
		res.end();
		return;
	}

	if (!req.files || Object.keys(req.files).length === 0) {
    	return res.status(400).send('No files were uploaded.');
  	}

	let video = req.files.video;
	let dir = `${__dirname}/media/${user}/${video.name}`;
	video.mv(dir, function(err) {
		if (err){
			console.log("Error uploading video: " + err.code);
			res.status(500);
			res.end();
		}
	//Add video to user video list
	})
})


app.listen(port, () => {
	fs.readFile(`${__dirname}/db/users.json`, (err, data) => {
		if (err){
			console.log(err);
		}
		console.log("DB loadaed");
		usersDB = JSON.parse(data);
	});

	console.log('HTTP Server listening')
})

app.post("/streaming", (req, res, next) => {
	res.status(501);
	res.end();

	//let vid = req.body.vid;
	let vid = 'Vid1';
	console.log(`Requested ${vid}`);
	var uri = url.parse(req.url).pathname;
	res.writeHead(200, { 'Access-Control-Allow-Origin': '*' });


	var filename = "./media/Vid1/index.m3u8";
	var source = "./media/Vid1/index.m3u8";


	fs.readFile(source, function(error, content) {
		if(error) {
			console.log(`Error loading video file in ${filepath + source} with ${error.code}`);
			res.status(500);
			res.end();
		}
		else{
			res.end(content, 'utf-8');

		}
	});

	res.end();
});

new hls(8081, {
    provider: {
        exists: (req, cb) => {
            const ext = req.url.split('.').pop();

            if (ext !== 'm3u8' && ext !== 'ts') {
                return cb(null, true);
            }

            fs.access(__dirname + req.url, fs.constants.F_OK, function (err) {
                if (err) {
                    console.log('File not exist');
                    return cb(null, false);
                }
                cb(null, true);
            });
        },
        getManifestStream: (req, cb) => {
            const stream = fs.createReadStream(__dirname + req.url);
            cb(null, stream);
        },
        getSegmentStream: (req, cb) => {
            const stream = fs.createReadStream(__dirname + req.url);
            cb(null, stream);
        }
    }
});

//Comprobat, funciona a nattech.fib.upc.edu:40330 sense VPN
