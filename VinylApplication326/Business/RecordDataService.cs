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
    }
}
