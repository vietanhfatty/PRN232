using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.DataAccess;

public class DBManager
{
    private static DBManager? _instance;
    private static readonly object _instanceLock = new object();

    private DBManager() { }

    public static DBManager Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new DBManager();
                }
                return _instance;
            }
        }
    }

    public HospitalManagementDbContext GetDbContext()
    {
        return new HospitalManagementDbContext();
    }
}
