using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    private string connectionString;

    void Start()
    {
        
        connectionString = GetDatabasePath();
    }

    public void InsertResearchAccount(string name, string email, string institution, string role)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "INSERT INTO ResearchAccounts (Name, Email, Institution, Role) " +
                              $"VALUES ('{name}', '{email}', '{institution}', '{role}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
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
    }

    public void UpdateResearchAccount(int accountId, string newName, string newEmail, string newInstitution, string newRole)
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

    public void DeleteResearchAccount(int accountId)
    {
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

    public string GetResearchAccountById(int accountId)
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

        public void AddPatient(string name, DateTime dateOfBirth, string gender, string horseName)
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
        
    public void RemovePatient(int patientId)
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
    
    public void UpdatePatient(int patientId, string name, DateTime dateOfBirth, string gender, string horseName)
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
    
    public List<string> GetAllPatients()
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

    
    public void AddSessionForPatient(int patientId, string sessionData)
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
    
    public void RemoveSessionForPatient(int patientId, int sessionId)
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
    
    public List<string> GetSessionsForPatient(int patientId)
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
    
    private string GetDatabasePath()
    {
    
        TextAsset config = Resources.Load<TextAsset>("DatabaseConfig");
        return config.text.Trim();
    }
    

    #endregion
}
