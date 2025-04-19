using Lib_System.Models;
using System.Collections.Generic;

public interface IPublisherService
{
    IEnumerable<PublisherViewModel> GetAllPublisherDetails();
    Publisher GetPublisherById(int id);
    int CreatePublisher(Publisher p);
    bool UpdatePublisher(Publisher p);
    bool DeletePublisher(int id);
}
