using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RenterCommunicationRepository : EntityBaseRepository<RenterCommunicationHistory>, IRenterCommunicationRepository
    {
        public RenterCommunicationRepository(RicDbContext context) : base(context)
        { }

        public IList<RenterCommunicationHistory> Find(int id)
        {
            return Context.RenterCommunicationHistory.AsNoTracking().Where(mch => mch.Id.Equals(id)).ToList();
        }

        public IQueryable<RenterCommunicationHistory> FindAll()
        {
            return Context.RenterCommunicationHistory;
        }

        public long Save(RenterCommunicationHistory memberComm)
        {
            RenterCommunicationHistory comm = null;
            if (memberComm.Id > 0)
            {
                comm = Context.RenterCommunicationHistory.FirstOrDefault(q => q.Id == memberComm.Id);
            }

            if (comm == null)
            {
                comm = new RenterCommunicationHistory();
                Context.RenterCommunicationHistory.Add(comm);
            }

            comm.CommunicationUtcdateTime = memberComm.CommunicationUtcdateTime;
            comm.CommunicationText = memberComm.CommunicationText;
            comm.CommunicationType = memberComm.CommunicationType;
            comm.CommunicationSentTo = memberComm.CommunicationSentTo;
            comm.RenterId = memberComm.RenterId;
            comm.RequestedBy = memberComm.RequestedBy;
            comm.IsSuccessfullySent = memberComm.IsSuccessfullySent;
            comm.BatchId = memberComm.BatchId;
            comm.MessageId = memberComm.MessageId;
            comm.HasRead = memberComm.HasRead ?? false;
            comm.AttachmentFileName = memberComm.AttachmentFileName;
            comm.ContentType = memberComm.ContentType;

            Context.SaveChanges();

            return comm.Id;
        }

       
    }
}
