using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Context _context;
        public MessageRepository(Context context)
        {
            _context = context;
        }
        public int AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
            return message.Id;
        }

        public void DeleteMessage(int id)
        {
            var result = _context.Messages.Find(id);
            if (result != null)
            {
                result.IsDeleted = true;
                _context.SaveChanges();
            }
        }

        public IQueryable<Message> GetAllMessage()
        {
            var result = _context.Messages;
            return result;
        }

        public Message GetMessage(int id)
        {
            var result = _context.Messages.Find(id);
            return result;
        }

        public void UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
        }
    }
}
