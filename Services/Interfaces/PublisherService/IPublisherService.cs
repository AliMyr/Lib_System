using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IPublisherService
    {
        IEnumerable<Publisher> GetAllPublishers();
        int CreatePublisher(Publisher publisher);
        bool UpdatePublisher(Publisher publisher);
        bool DeletePublisher(int id);
    }
}
