using VinylApplication326.DAO;
using VinylApplication326.Models;

namespace VinylApplication326.Services
{
    public class RecordDataService
    {
        RecordDao dao;

        public RecordDataService()
        {
            dao = new RecordDao();
        }

        public List<RecordModel> readRecords() {

            List<RecordModel> records = dao.GetRecords();
            
            return records;
        }

        public void favoriteRecord(int recordId)
        {
            dao.FavoriteToggle(recordId);
        }

        public bool createRecord(RecordModel model)
        {
            return dao.createRecord(model);
        }

        public bool deleteRecord(int recordId, int userId)
        {
            return dao.deleteRecord(recordId, userId);
        }

        public RecordModel getRecordByIdAndUserId(int recordId, int userId)
        {
            return dao.getRecordByIdAndUserId(recordId, userId);
        }

        public bool doEdit(RecordModel model)
        {
            return dao.doEdit(model);
        }
    }
}
