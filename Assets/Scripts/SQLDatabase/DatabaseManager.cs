using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public static class DatabaseManager 
{
    private static string connectionString;
    private static  bool m_isLoggedIn; 
    public static void SetupDatabasePath()
    {
        
        connectionString = GetDatabasePath();
    }

    public static int InsertResearchAccount(string name, string email, string institution, string role, string password)
    {
        // Check if the user already exists
        if (IsUserExists(email))
        {
            Debug.LogWarning("User with this email already exists.");
            return -1; // Return -1 to indicate failure
        }

        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;
        int accountId = -1; // Default value if insertion fails

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "INSERT INTO ResearchAccounts (Name, Email, Institution, Role, Password) " +
                              $"VALUES ('{name}', '{email}', '{institution}', '{role}', '{password}'); " +
                              "SELECT last_insert_rowid();"; // SQLite function to get the last inserted row ID
            dbCmd.CommandText = sqlQuery;

            // ExecuteScalar returns the first column of the first row in the result set
            object result = dbCmd.ExecuteScalar();
            if (result != null)
            {
                accountId = Convert.ToInt32(result);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error inserting research account: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return accountId;
    }
    
    public static void ResetPassword(string email, string newPassword)
    {
        // Check if the user exists
        if (!IsUserExists(email))
        {
            Debug.LogWarning("User with this email does not exist.");
            return;
        }

        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE ResearchAccounts SET Password='{newPassword}' WHERE Email='{email}'";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error resetting password: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static void UpdateResearchAccount(int accountId, string newName, string newEmail, string newInstitution, string newRole)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;
        
        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE ResearchAccounts SET Name='{newName}', Email='{newEmail}', Institution='{newInstitution}', Role='{newRole}' " +
                              $"WHERE ID={accountId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating research account: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static void DeleteAccountByEmail(string email)
    {
        // Check if the user exists
        if (!IsUserExists(email))
        {
            Debug.LogWarning("User with this email does not exist.");
            return;
        }

        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"DELETE FROM ResearchAccounts WHERE Email='{email}'";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting account: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }

    public static void DeleteResearchAccount(int accountId)
    {
        if (!IsUserExists(accountId))
        {
            Debug.LogWarning("Research account with this ID does not exist.");
            return;
        }

        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"DELETE FROM ResearchAccounts WHERE ID={accountId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting research account: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }

    public static string GetResearchAccountById(int accountId)
    {

        string result = null;
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT * FROM ResearchAccounts WHERE ID={accountId}";
            dbCmd.CommandText = sqlQuery;

            using (IDataReader reader = dbCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = $"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}";
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error retrieving research account: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return result;
    }
    
    public static int GetAccountIdByEmail(string email)
    {
        int accountId = -1; // Default value if account not found
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT ID FROM ResearchAccounts WHERE Email='{email}'";
            dbCmd.CommandText = sqlQuery;
            
            object result = dbCmd.ExecuteScalar();
            if (result != null)
            {
                accountId = Convert.ToInt32(result);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error retrieving account ID by email: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return accountId;
    }
    
    public static bool Login(string email, string password)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;
        bool loginSuccessful = false;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT COUNT(*) FROM ResearchAccounts WHERE Email='{email}' AND Password='{password}'";
            dbCmd.CommandText = sqlQuery;

           
            int count = Convert.ToInt32(dbCmd.ExecuteScalar());
            loginSuccessful = count > 0;
            m_isLoggedIn = loginSuccessful;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during login: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return loginSuccessful;
    }
    
    public static void AddPatient(string name, DateTime dateOfBirth, string gender, string horseName)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"INSERT INTO Patients (Name, DateOfBirth, Gender, HorseName) " +
                              $"VALUES ('{name}', '{dateOfBirth.ToString("yyyy-MM-dd")}', '{gender}', '{horseName}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error adding patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
        
    public static void RemovePatient(int patientId)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"DELETE FROM Patients WHERE ID={patientId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error removing patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static void UpdatePatient(int patientId, string name, DateTime dateOfBirth, string gender, string horseName)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE Patients SET Name='{name}', DateOfBirth='{dateOfBirth.ToString("yyyy-MM-dd")}', " +
                              $"Gender='{gender}', HorseName='{horseName}' WHERE ID={patientId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static List<string> GetAllPatients()
    {
        List<string> patients = new List<string>();
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "SELECT * FROM Patients";
            dbCmd.CommandText = sqlQuery;

            using (IDataReader reader = dbCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string patientInfo = $"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)}, Date of Birth: {reader.GetDateTime(2)}, " +
                                         $"Gender: {reader.GetString(3)}, Horse Name: {reader.GetString(4)}";
                    patients.Add(patientInfo);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error retrieving patients: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return patients;
    }
    
    public static void AddSessionForPatient(int patientId, string sessionData)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"INSERT INTO Sessions (PatientId, SessionData) VALUES ({patientId}, '{sessionData}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error adding session for patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static void RemoveSessionForPatient(int patientId, int sessionId)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"DELETE FROM Sessions WHERE PatientId={patientId} AND SessionId={sessionId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error removing session for patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
    
    public static List<string> GetSessionsForPatient(int patientId)
    {
        List<string> sessions = new List<string>();
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT * FROM Sessions WHERE PatientId={patientId}";
            dbCmd.CommandText = sqlQuery;

            using (IDataReader reader = dbCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string sessionData = reader.GetString(2); // Assuming session data is stored in the 3rd column
                    sessions.Add(sessionData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error retrieving sessions for patient: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return sessions;
    }

    #region Private
    
    private static string GetDatabasePath()
    {
    
        TextAsset config = Resources.Load<TextAsset>("DatabaseConfig");
        return config.text.Trim();
    }
    
    private static bool IsUserExists(string email)
    {
        bool exists = false;
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT COUNT(*) FROM ResearchAccounts WHERE Email='{email}'";
            dbCmd.CommandText = sqlQuery;

           
            int count = Convert.ToInt32(dbCmd.ExecuteScalar());
            exists = count > 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error checking if user exists: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return exists;
    }
    
    private static bool IsUserExists(int accountId)
    {
        bool exists = false;
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT COUNT(*) FROM ResearchAccounts WHERE ID={accountId}";
            dbCmd.CommandText = sqlQuery;
            
            int count = Convert.ToInt32(dbCmd.ExecuteScalar());
            exists = count > 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error checking if account exists: {e.Message}");
        }
        finally
        {
            if (dbCmd != null)
            {
                dbCmd.Dispose();
            }
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return exists;
    }
    
    #endregion
}
