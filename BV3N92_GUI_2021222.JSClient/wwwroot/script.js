let members = [];
let connection = null;

let memberIDToUpdate = -1;
let keypairs = [];

getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:41126/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("PartyMemberCreated", (user, message) => {
        getdata();
    });

    connection.on("PartyMemberDeleted", (user, message) => {
        getdata();
    });

    connection.on("PartyMemberUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });

    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

async function getdata() {
    await fetch('http://localhost:41126/partymember').then(x => x.json()).then(y => {
        members = y;
        //console.log(members);
        display();
    });
}

async function getnoncruddata() {
    await fetch('http://localhost:41126/stat/avgageofmembersperparty').then(x => x.json()).then(y => {
        keypairs = y;
        //console.log(members);
        avgagedisplay();
    });
}

function avgagedisplay() {
    document.getElementById('avgagearea').innerHTML = "";
    keypairs.forEach(x => {
        document.getElementById('avgagearea').innerHTML +=
            "<tr><td>" + x.key + "</td><td>" + x.value + "</td></tr>";
        console.log(x.key);
    });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    members.forEach(x => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + x.memberID + "</td><td>" + x.lastName + "</td><td>" + x.age + "</td><td>" + x.partyID +
            "</td><td>" + `<button type="button" onclick="remove(${x.memberID})">Delete</button>` +
            `<button type="button" onclick="showupdate(${x.memberID})">Update</button>` + "</td></tr>";
        console.log(x.lastName);
    });
}

function remove(id) {
    fetch('http://localhost:41126/partymember/' + id, {
        method: 'delete',
        headers: { 'content-type': 'application/json', },
        body: null}).then(response => response).then(data => { console.log('success:', data); getdata(); }).catch((error) => { console.error('error:', error); });
}

function showupdate(id) {
    document.getElementById('membernametoupdate').value = members.find(m => m.memberID == id)['lastName'];
    document.getElementById('memberagetoupdate').value = members.find(m => m.memberID == id)['age'];
    document.getElementById('partyidtoupdate').value = members.find(m => m.memberID == id)['partyID'];
    document.getElementById('updateformdiv').style.display = 'flex';
    document.getElementById('formdiv').style.display = 'none';
    memberIDToUpdate = id;
}

function avgageperparty() {
    getnoncruddata();
    document.getElementById('noncrudaction').style.display = 'none';
    document.getElementById('formdiv').style.display = 'none';
    document.getElementById('updateformdiv').style.display = 'none';
    document.getElementById('avgageformdiv').style.display = 'flex';
    document.getElementById('membertablediv').style.display = 'none';
}

function loadmemberstable() {
    document.getElementById('formdiv').style.display = 'flex';
    document.getElementById('avgageformdiv').style.display = 'none';
    document.getElementById('noncrudaction').style.display = 'flex';
    document.getElementById('membertablediv').style.display = 'flex';
}

function update() {
    document.getElementById('updateformdiv').style.display = 'none';
    document.getElementById('formdiv').style.display = 'flex';
    let name = document.getElementById('membernametoupdate').value;
    let age = document.getElementById('memberagetoupdate').value;
    let partyid = document.getElementById('partyidtoupdate').value;

    fetch('http://localhost:41126/partymember', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify({ memberID: memberIDToUpdate, lastName: name, age: age, partyID: partyid }),
    }).then(response => response).then(data => { console.log('Success:', data); getdata(); }).catch((error) => { console.error('Error:', error); });
}

function create() {
    let name = document.getElementById('membername').value;
    let age = document.getElementById('memberage').value;
    let partyid = document.getElementById('partyid').value;

    fetch('http://localhost:41126/partymember', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify({ lastName: name, age: age, partyID: partyid }),
    }).then(response => response).then(data => { console.log('Success:', data); getdata(); }).catch((error) => { console.error('Error:', error); });
}