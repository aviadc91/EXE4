using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using server.BL;
using server.Controllers;
using System.Globalization;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

//*****************************start U S E R *****************************//
	public int Insert(User user)
	{

		SqlConnection con;
		SqlCommand cmd;

		try
		{
			con = connect("myProjDB"); // create the connection
		}
		catch (Exception ex)
		{
			// write to log
			throw (ex);
		}

		cmd = CreateInsertWithStoredProcedure("CreateUser", con, user);             // create the command

		try
		{
			int numEffected = cmd.ExecuteNonQuery(); // execute the command
			return numEffected;
		}
		catch (Exception ex)
		{
			// write to log
			throw (ex);
		}

		finally
		{
			if (con != null)
			{
				// close the db connection
				con.Close();
			}
		}

	}

	private SqlCommand CreateInsertWithStoredProcedure(String spName, SqlConnection con, User user)
	{

		SqlCommand cmd = new SqlCommand(); // create the command object

		cmd.Connection = con;              // assign the connection to the command object

		cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

		cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

		cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

		cmd.Parameters.AddWithValue("@firstName", user.FirstName);

		cmd.Parameters.AddWithValue("@familyName", user.FamilyName);
		cmd.Parameters.AddWithValue("@password", user.Password);
		cmd.Parameters.AddWithValue("@email", user.Email);

		return cmd;
	}

	public User LogInUser(string email, string password)
	{

		SqlConnection con;
		SqlCommand cmd;

		try
		{
			con = connect("myProjDB"); // create the connection
		}
		catch (Exception ex)
		{
			// write to log
			throw (ex);
		}

		cmd = getLogedUserByStoredProcedure("GetUserByEmailAndPassword", con, email, password);

		User u = new User();
		try
		{
			SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			if (dataReader.Read())
			{

				u.FirstName = dataReader["firstName"].ToString(); 
				u.FamilyName = dataReader["familyName"].ToString();
				//u.Password = dataReader["password"].ToString();
				u.Email = dataReader["email"].ToString();
				u.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
				u.IsAdmin = Convert.ToBoolean(dataReader["IsAdmin"]);

			}
			else
			{
				// No matching user found, throw an exception or handle accordingly
				throw new InvalidOperationException("Invalid email or password");
			}

		}
		catch (Exception ex)
		{
			throw (ex);
		}

		finally
		{
			if (con != null)
			{
				// close the db connection
				con.Close();
			}
		}

		return u;

	}

	private SqlCommand getLogedUserByStoredProcedure(String spName, SqlConnection con, string email, string password)
	{

		SqlCommand cmd = new SqlCommand(); // create the command object

		cmd.Connection = con;              // assign the connection to the command object

		cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

		cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

		cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

		cmd.Parameters.AddWithValue("@inputEmail", email);
		cmd.Parameters.AddWithValue("@inputPassword", password);

		return cmd;
	}


	//--------------------------------------------------------------------------------------------------
	// This method update a user to the user table 
	//--------------------------------------------------------------------------------------------------
	public int Update(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("UpdateUser", con, user);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String email, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = email;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
        cmd.Parameters.AddWithValue("@familyName", user.FamilyName);
        cmd.Parameters.AddWithValue("@password", user.Password);
		cmd.Parameters.AddWithValue("@email", user.Email);
		cmd.Parameters.AddWithValue("@isActive", user.IsActive);
		cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);


		return cmd;
    }


    public List<User> ReadUser()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<User> users = new List<User>();

        cmd = buildReadStoredProcedureCommand(con, "ReadAllUsers");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            User u = new User();
            u.FirstName = dataReader["FirstName"].ToString();
            u.FamilyName = dataReader["FamilyName"].ToString();
            u.Password = dataReader["Password"].ToString();
            u.Email= dataReader["Email"].ToString();
			u.IsActive = Convert.ToBoolean(dataReader["IsActive"].ToString());
			u.IsAdmin = Convert.ToBoolean(dataReader["IsAdmin"].ToString());
			users.Add(u);
		}

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return users;


    }


    SqlCommand buildReadStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }

    public int DeleteUser(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateDeleteWithStoredProcedure("DeleteUser", con, email);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


	private SqlCommand CreateDeleteWithStoredProcedure(String spName, SqlConnection con, string email)
	{

		SqlCommand cmd = new SqlCommand(); // create the command object

		cmd.Connection = con;              // assign the connection to the command object

		cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

		cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

		cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

		cmd.Parameters.AddWithValue("@email", email);

		return cmd;
	}

    //*****************************end U S E R *****************************//

    //*****************************start F L A T *****************************//
    public int InsertFlat(Flat flat)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertFlatWithStoredProcedure("CreateFlat", con, flat);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateInsertFlatWithStoredProcedure(String spName, SqlConnection con, Flat flat)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@city", flat.City);

        cmd.Parameters.AddWithValue("@address", flat.Address);
        cmd.Parameters.AddWithValue("@price", flat.Price);
        cmd.Parameters.AddWithValue("@numOfRooms", flat.NumOfRooms);

        return cmd;
    }





}
