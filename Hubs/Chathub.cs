using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SignalRDenemesi.Interface;
using SignalRDenemesi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Runtime.ConstrainedExecution;
using System;
using System.Diagnostics.Metrics;

namespace SignalRDenemesi.Hubs
{
    public class Chathub : Hub
    {
        #region Propertyler

        //### Önemli `Hub` Property'leri

        //1. **`Clients`**
        //   - İstemcilerle etkileşimde bulunmanıza olanak tanır. `Clients` nesnesi üzerinden belirli bir istemciye, tüm istemcilere veya belirli gruplara mesaj gönderebilirsiniz.

        //   - **Kullanım Örnekleri:**
        //     - `Clients.All`: Tüm bağlı istemcilere mesaj gönderir.
        //     - `Clients.Caller`: Yalnızca çağrıyı yapan istemciye mesaj gönderir.
        //     - `Clients.Others`: Çağrıyı yapan istemci hariç tüm istemcilere mesaj gönderir.
        //     - `Clients.User("userId")`: Belirli bir kullanıcıya mesaj gönderir.

        //2. **`Context`**
        //   - Bağlantı hakkında bilgi içerir.İstemcinin bağlantı kimliği (connection ID), istemcinin kimliği veya çağrı yapan istemci hakkında diğer bilgilere erişim sağlar.

        //   - **Kullanım Örnekleri:**
        //     - `Context.ConnectionId`: İstemcinin benzersiz bağlantı kimliği.
        //     - `Context.User`: Çağrıyı yapan istemci hakkında kullanıcı bilgisi.
        //     - `Context.Items`: Her bağlantıya özel verileri saklayabileceğiniz bir sözlük (key-value pair).

        //3. **`Groups`**
        //   - İstemcileri gruplara eklemek veya gruplardan çıkarmak için kullanılır.Gruplar, belirli istemcilere toplu mesaj göndermek için yararlıdır.

        //   - **Kullanım Örnekleri:**
        //     - `AddToGroupAsync(connectionId, groupName)`: Belirli bir istemciyi (bağlantı ID ile) bir gruba ekler.
        //     - `RemoveFromGroupAsync(connectionId, groupName)`: Belirli bir istemciyi bir gruptan çıkarır.

        //4. **`Caller`**
        //   - `Clients.Caller` property'sinin kısa yolu. Sadece çağrıyı yapan istemciye mesaj göndermek için kullanılır.
        #endregion

        #region Methodlar
        //SignalR `Hub` Sınıfının Önemli Metotları

        //1.**OnConnectedAsync**
        //- Bir istemci SignalR sunucusuna bağlandığında çalıştırılan metodur.Her yeni bağlantı olduğunda bu metot devreye girer ve bağlantı kurulan istemciyle ilgili işlemler yapmanıza olanak tanır.

        //2.**OnDisconnectedAsync**
        //- Bir istemci bağlantıyı sonlandırdığında ya da bağlantısı koptuğunda çalıştırılan metodur.Bağlantı kesildiğinde temizleme işlemleri veya bilgilendirme için kullanılır.

        //3. **SendAsync**
        //   - İstemcilerle iletişim kurmak ve onlara veri göndermek için kullanılır. `Clients` üzerinden çağrılır ve belirli bir istemciye ya da tüm istemcilere mesaj iletilmesini sağlar.

        //   - **Kullanım Alanları:**
        //     - İstemcilere gerçek zamanlı mesaj gönderme
        //     - Belirli gruplara veri yayını
        //     - Yalnızca bağlantı kuran istemciye yanıt gönderme

        //4. **AddToGroupAsync**
        //   - Bir istemciyi belirli bir gruba ekler. Bu metod ile gruplar oluşturabilir ve istemcileri gruplara dahil edebilirsiniz.

        //   - **Kullanım Alanları:**
        //     - İlgili istemcileri gruplara ayırarak mesajları grup bazlı iletme
        //     - Kullanıcıları role dayalı gruplara ayırma

        //5. **RemoveFromGroupAsync**
        //   - Bir istemciyi belirli bir gruptan çıkarır. İstemciler gruptan çıkarıldığında artık bu gruba gönderilen mesajları almazlar.

        //   - **Kullanım Alanları:**
        //     - Gruplardan istemci çıkarma
        //     - Belirli bir etkinlik sona erdiğinde istemcilerin gruptan ayrılmasını sağlama

        //### Metotların Özeti

        //| Metot               | Açıklama                                                                                              |
        //|---------------------|------------------------------------------------------------------------------------------------------|
        //| **OnConnectedAsync**     | İstemci sunucuya bağlandığında çalışır, bağlantı kurulan istemciyle ilgili işlemler yapılır.          |
        //| **OnDisconnectedAsync**  | İstemci bağlantıyı sonlandırdığında veya koptuğunda çalışır, temizleme ve kapanış işlemleri yapılır. |
        //| **SendAsync**            | İstemcilere mesaj göndermek için kullanılır.                                                        |
        //| **AddToGroupAsync**      | İstemciyi bir gruba ekler.                                                                         |
        //| **RemoveFromGroupAsync** | İstemciyi bir gruptan çıkarır.                                                                     |

        #endregion

        //private readonly AppDbContext _context;
        //private readonly List<string> connectionId;
        //public Chathub(AppDbContext context, List<string> connectionId)
        //{
        //    _context = context;
        //    this.connectionId = connectionId;
        //}
        //List<string> connectionList;

        //public Chathub(List<string> connectionList)
        //{
        //    this.connectionList = connectionList;
        //}
        private readonly AppDbContext appDb;

        public Chathub(AppDbContext appDb)
        {
            this.appDb = appDb;
        }

        public async Task Connection()
        {
            // Bağlanan istemciye hoş geldin mesajı gönderebiliriz
            await Clients.Caller.SendAsync("OnConnected", Context.ConnectionId);
        }
        public async Task SendMessageMethods(string user, string message)
        {
            await Clients.All.SendAsync("SendMessage", Context.ConnectionId, user, message);
        }
        public async Task AllExceptMethods(string Id, string user, string message)
        {
            await Clients.AllExcept(Id).SendAsync("AllExcept", user, message);
        }
        public async Task ClientMethods(string Id, string user, string message)
        {
            await Clients.Client(Id).SendAsync("Client", Context.ConnectionId, user, message);
        }
        public async Task ListAdd(string connectionId)
        {
            try
            {
                await appDb.Users.AddAsync(new User() { UserId = connectionId });
                await appDb.SaveChangesAsync();
                await Clients.Clients(Context.ConnectionId).SendAsync("ListAdd", $"{connectionId} Eklendi");
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task ClientsListMethods(string user, string message)
        {
            await Clients.Clients(appDb.Users.Select(p => p.UserId)).SendAsync("ClientsList", Context.ConnectionId, user, message);
        }
        public async Task JoinGroupMethods(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("JoinGroup", $"{Context.ConnectionId} Gruba Kayıt Olundu.");
        }
        public async Task LeaveGroupMethods(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("LeaveGroup", $"{Context.ConnectionId} Gruptan Çıkış Yaptı.");
        }
        public async Task SendGroupMessageMethods(string user,string message,string groupName)
        {
            await Clients.Group(groupName).SendAsync("SendGroupMessage",Context.ConnectionId,user,message);
        }
    }
}
