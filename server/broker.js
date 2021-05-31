const aedes = require('aedes')()
const server = require('net').createServer(aedes.handle)
const port = 8081

server.listen(port, function () {
  console.log("aedes broker running")
})
