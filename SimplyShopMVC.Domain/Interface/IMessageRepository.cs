using SimplyShopMVC.Domain.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface;

public interface IMessageRepository
{
    int AddMessage(Message message);
    void UpdateMessage(Message message);
    void DeleteMessage(int id);
    Message GetMessage(int id);
    IQueryable<Message> GetAllMessage();
}
