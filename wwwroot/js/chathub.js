//"use strict";

// Kopar ise tekrar tekrar baglanmaya calisir."withAutomaticReconnect()"
// Kopar ise array de ki milisaniyeler icersinde tekrar dener."withAutomaticReconnect([1000,2000,3000,4000,5000])"
//onreconnecting yeniden baglanma girisimlerini baslatmadan once firlatilan/tetiklenen eventtir.
//onreconnected yeniden baglanti gerceklestiginde tetiklenen fonksiyondur.
//onclose yeniden baglanti girismlerinin sonucsuz kaldigi durumlarda firlatilir.

//1. connection.start()
//SignalR bağlantısını başlatmak için kullanılır.Bağlantı kurulduğunda, istemci ile sunucu arasındaki iletişim başlar.
//    2. connection.stop()
//Bağlantıyı sonlandırmak için kullanılır.Bu, sunucu ile istemci arasındaki mevcut bağlantıyı keser.
//    3. connection.invoke(methodName, ...args)
//Sunucudaki bir SignalR hub metodunu çağırmak için kullanılır.Bu metot, sunucuya parametreler göndererek çağırdığınız metodu çalıştırır.
//    4. connection.on(methodName, callback)
//Sunucudan bir SignalR hub metodundan gelen mesajları almak için kullanılır.Bu metot, sunucudan gönderilen mesajlara karşılık gelen işlemi belirlemek için kullanılır.
//    5. connection.off(methodName, callback)
//Daha önce connection.on() ile kaydedilen bir metodu iptal etmek için kullanılır.Bu, belirli bir hub metodunu dinlemeyi bırakmanızı sağlar.
//    6. connection.onclose(callback)
//Bağlantı kapandığında veya herhangi bir hata nedeniyle kesildiğinde tetiklenen bir olayı işler.callback fonksiyonu, bağlantı kapandığında çağrılır.
//    7. connection.reconnect()
//Otomatik yeniden bağlanma etkinse ve bağlantı kesildiyse, SignalR otomatik olarak yeniden bağlanmaya çalışır.
//8. connection.state
//Bağlantının mevcut durumunu döndürür.Durumlar:
//Disconnected(0)
//Connecting(1)
//Connected(2)
//Reconnecting(3)
//Dönüş Tipi: HubConnectionState


"use strict";

function throwError(error) {
    return listvalue.innerHTML += `<p class="text-secondary m-0 p-0"><b> Connection ID </b> : <span class="text-secondary">${error}</span></p>`;
}


var htmlconnectionid = document.querySelector('#connectionId');


// #region Connection Baslangic

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .withAutomaticReconnect()
    .build();
var connectionID;
connection.start().then(function () {
    connection.on("OnConnected", function (connectionId) {
        connectionID = this.connectionId;
        htmlconnectionid.innerHTML =
            `<p class="text-secondary m-0 p-0"><b> Connection ID </b> : <span class="text-secondary">${connectionId}</span></p>`
    });
    connection.on("SendMessage", function (connectionId, user, message) {
        let listvalue = document.querySelector('#container-body > div > div:nth-child(3) > table > tbody');
        listvalue.innerHTML += `
        <tr>
         <td>${connectionId}</td>
         <td>${user}</td>
         <td>${message}</td>
        </tr>
        `;
    });
    connection.on("AllExcept", function (user, message) {
        let listvalue = document.querySelector('#container-body-1 > div > div:nth-child(3) > table > tbody');
        listvalue.innerHTML += `
        <tr>
        <td>${user}</td>
         <td>${message}</td>
        </tr>
        `;
    });
    connection.on("Client", function (id, user, message) {
        let listvalue = document.querySelector('#container-body-2 > div > div:nth-child(3) > table > tbody');
        listvalue.innerHTML += `
        <tr>
        <td>${id}</td>
        <td>${user}</td>
         <td>${message}</td>
        </tr>
        `;
    });
    connection.on("ClientsList", function (id, user, message) {
        let listvalue = document.querySelector('#container-body-3 > div > div:nth-child(3) > table > tbody');
        listvalue.innerHTML += `
        <tr>
        <td>${id}</td>
        <td>${user}</td>
         <td>${message}</td>
        </tr>
        `;
    });
    connection.on("ListAdd", function (message) {
        alert(`${message}`);
    });
    connection.on("JoinGroup", function (message) {
        alert(`${message} `);
    });
    connection.on("LeaveGroup", function (message) {
        alert(`${message} `);
    });
    connection.on("SendGroupMessage", function (id,user, message) {
        let listvalue = document.querySelector('#container-body-4 > div > div:nth-child(3) > table > tbody');
        listvalue.innerHTML += `
        <tr>

            <td>${id}</td>
            <td>${user}</td>
         <td>${message}</td>
        </tr>
        `;
    });

    connection.invoke("Connection");
});

//#endregion


