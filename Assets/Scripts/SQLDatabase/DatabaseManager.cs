using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;
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

    public void InsertPatent(string title, string description, string inventor, string filedDate, string status)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "INSERT INTO Patents (Title, Description, Inventor, FiledDate, Status) " +
                              $"VALUES ('{title}', '{description}', '{inventor}', '{filedDate}', '{status}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error inserting patent: {e.Message}");
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

    public void UpdatePatent(int patentId, string newTitle, string newDescription, string newInventor, string newFiledDate, string newStatus)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE Patents SET Title='{newTitle}', Description='{newDescription}', Inventor='{newInventor}', " +
                              $"FiledDate='{newFiledDate}', Status='{newStatus}' WHERE ID={patentId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating patent: {e.Message}");
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

    public void DeletePatent(int patentId)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"DELETE FROM Patents WHERE ID={patentId}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting patent: {e.Message}");
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

    public string GetPatentById(int patentId)
    {
        string result = null;
        IDbConnection dbConnection = null;
        IDbCommand dbCmd = null;

        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT * FROM Patents WHERE ID={patentId}";
            dbCmd.CommandText = sqlQuery;

            using (IDataReader reader = dbCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = $"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}, {reader.GetString(5)}, {reader.GetString(6)}";
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error retrieving patent: {e.Message}");
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

    #region Private
    
    private string GetDatabasePath()
    {
    
        TextAsset config = Resources.Load<TextAsset>("DatabaseConfig");
        return config.text.Trim();
    }
    

    #endregion
}
