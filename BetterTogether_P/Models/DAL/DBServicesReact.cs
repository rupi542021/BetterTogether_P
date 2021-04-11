﻿using project_classes.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace BetterTogetherProj.Models.DAL
{
    public class DBServicesReact
    {
        //public SqlDataAdapter da;
        //public DataTable dt;

        public DBServicesReact()
        {

        }
        public SqlConnection connect(String conString)
        {

            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = CommandSTR;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd;
        }

        //public Student checkStudentRuppin(string email)
        //{
        //    SqlConnection con = null;
        //    Student stud = new Student();

        //    try
        //    {
        //        con = connect("DBConnectionString"); 

        //        String selectSTR = "SELECT * FROM Ruppin_StudentsData_P";
        //        SqlCommand cmd = new SqlCommand(selectSTR, con);
        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dr.Read())
        //        {
        //            if (email == (string)dr["email"])
        //            {
        //                bool alreadyExist = checkIfExist(email);
        //                if (alreadyExist == false)
        //                {
        //                    stud.Mail = (string)dr["email"];
        //                    stud.Fname = (string)(dr["firstName"]);
        //                    stud.Lname = (string)(dr["lastName"]);
        //                    stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
        //                    stud.Dep = getStudDep(Convert.ToInt32(dr["department"]));
        //                    stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);
        //                    return stud;
        //                }

        //                else {
        //                    Exception ex = new Exception("email already exists");  
        //                    throw (ex);
        //                }
        //            }
        //        }
        //        //stud.Mail = null;
        //        //return stud;
        //        Exception e = new Exception("email not found");
        //        throw (e);
        //    }
        //    catch (Exception ex)
        //    {
        //        writeToLog(ex);
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }

        //    }
        //}

        public Student checkStudentRuppin(string email)
        {
            SqlConnection con = null;
            Student stud = new Student();

            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM Ruppin_StudentsData_P where [email] = '" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == true)
                {
                    bool alreadyExist = checkIfExist(email);
                    if (alreadyExist == false)
                    {
                        while (dr.Read())
                        {
                            stud.Mail = (string)dr["email"];
                            stud.Fname = (string)(dr["firstName"]);
                            stud.Lname = (string)(dr["lastName"]);
                            stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
                            stud.Dep = getStudDep(Convert.ToInt32(dr["department"]));
                            stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);

                        }
                        return stud;
                    }

                    else
                    {
                        Exception ex = new Exception("email already exists");
                        throw (ex);
                    }

                }
                else
                {
                    Exception e = new Exception("email not found");
                    throw (e);
                }

            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public Student checkStudentLogin(string email,string password)
        {
            SqlConnection con = null;
            Student stud = new Student();

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM student_P where mail='"+ email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        if (password == (string)dr["password"])
                        {
                            stud.Mail = (string)dr["mail"];
                            stud.Password = (string)dr["password"];
                            stud.Fname = (string)(dr["firstName"]);
                            stud.Lname = (string)(dr["lastName"]);
                            stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
                            stud.Dep = getStudDep(Convert.ToInt32(dr["departmentCode"]));
                            stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);
                            stud.HomeTown = getResidenceH((string)(dr["homeTown"]));
                            stud.AddressStudying = getResidenceS((string)(dr["adrressStudying"]));
                            stud.PersonalStatus = (string)(dr["personalStatus"]);
                            stud.IsAvailableCar = Convert.ToBoolean(dr["isAvailableCar"]);
                            stud.IntrestedInCarPool = Convert.ToBoolean(dr["intrestedInCarPool"]);
                            stud.Photo = (string)(dr["photo"]);
                            stud.Gender = (string)(dr["gender"]);
                            stud.RegistrationDate = Convert.ToDateTime(dr["registrationDate"]);
                            stud.ActiveStatus = Convert.ToBoolean(dr["active"]);
                            stud.Plist = GetPlistByUser((string)dr["mail"]);
                            stud.Hlist = GetHlistByUser((string)dr["mail"]);
                            stud.Friendslist = GetFriendsListByUser(((string)dr["mail"]));
                            return stud;
                        }
                            stud.Mail = (string)dr["mail"];
                            stud.Fname = (string)(dr["firstName"]);
                            stud.Lname = (string)(dr["lastName"]);
                        //stud.Password = null;
                        //return stud;
                        Exception ex = new Exception("incorrect password");
                        throw(ex);
                    }
                 }

                //stud.Mail = null;
                //return stud;
                Exception e = new Exception("email not found");
                throw (e);

            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public bool checkIfExist(string email)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT mail FROM student_P where [mail] ='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.HasRows)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public Department getStudDep(int DepID)
        {
            SqlConnection con = null;
            Department Dep = new Department();

            try
            {
                con = connect("DBConnectionString"); 

                String selectSTR = "SELECT * FROM department_P where departmentCode="+DepID;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    //if (DepID == Convert.ToInt32(dr["departmentCode"]))
                    {
                        Dep.DepartmentCode = Convert.ToInt32(dr["departmentCode"]);
                        Dep.DepartmentName = (string)(dr["departmentName"]);
                    }
                }
                return Dep;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        
        public Residence getResidenceH(string ResidenceName)
        {
            SqlConnection con = null;
            Residence res = new Residence();
            try
            {
                res = res.Read(ResidenceName);
                return res;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public Residence getResidenceS(string ResidenceName)
        {
            SqlConnection con = null;
            Residence res = new Residence();
            try
            {
                res = res.Read(ResidenceName);
                return res;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<Pleasure> GetPlistByUser(string email)
        {
            SqlConnection con = null;
            List<Pleasure> userPList = new List<Pleasure>();

            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM student_pleasure_P INNER JOIN pleasure_P ON pleasurepCode = pCode where studentmail = '"+email+"'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Pleasure p = new Pleasure();
                    p.Pcode = Convert.ToInt32(dr["pCode"]);
                    p.Pname = (string)(dr["pName"]);
                    p.Picon = (string)(dr["pIcon"]);
                    userPList.Add(p);
                }
                return userPList;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<string> GetFriendsListByUser(string email)
        {
            SqlConnection con = null;
            List<string> userFriendsList = new List<string>();

            try
            {
                con = connect("DBConnectionString"); 

                String selectSTR = "SELECT sf.student2Mail FROM student_favorites_P sf where sf.Student1Mail ='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

               
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); 

                while (dr.Read())
                {
                    userFriendsList.Add((string)dr["student2Mail"]);
                }
                return userFriendsList;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<Student> GetAllFavorites(string email)
        {
            SqlConnection con = null;
            List<Student> userFriendsList = new List<Student>();
            Student stud = new Student();
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT sf.student2Mail FROM student_favorites_P sf where sf.Student1Mail ='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);


                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                  stud = GetStudentDetails((string)dr["student2Mail"]);
                  userFriendsList.Add(stud);
                }
                var OrdereduserFriendsList = userFriendsList.OrderBy(x => x.Fname).ToList();
                return OrdereduserFriendsList;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public Student GetStudentDetails(string email)
        {
            SqlConnection con = null;
            Student stud = new Student();

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM student_P where mail='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               
                    while (dr.Read())
                    {                    
                            stud.Mail = (string)dr["mail"];
                            stud.Password = (string)dr["password"];
                            stud.Fname = (string)(dr["firstName"]);
                            stud.Lname = (string)(dr["lastName"]);
                            stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
                            stud.Dep = getStudDep(Convert.ToInt32(dr["departmentCode"]));
                            stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);
                            stud.HomeTown = getResidenceH((string)(dr["homeTown"]));
                            stud.AddressStudying = getResidenceS((string)(dr["adrressStudying"]));
                            stud.PersonalStatus = (string)(dr["personalStatus"]);
                            stud.IsAvailableCar = Convert.ToBoolean(dr["isAvailableCar"]);
                            stud.IntrestedInCarPool = Convert.ToBoolean(dr["intrestedInCarPool"]);
                            stud.Photo = (string)(dr["photo"]);
                            stud.Gender = (string)(dr["gender"]);
                            stud.RegistrationDate = Convert.ToDateTime(dr["registrationDate"]);
                            stud.ActiveStatus = Convert.ToBoolean(dr["active"]);
                            stud.Plist = GetPlistByUser((string)dr["mail"]);
                            stud.Hlist = GetHlistByUser((string)dr["mail"]);
                            stud.Friendslist = GetFriendsListByUser(((string)dr["mail"]));
                    }
                return stud;
            }

            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }


        public List<Hobby> GetHlistByUser(string email)
        {
            SqlConnection con = null;
            List<Hobby> userHList = new List<Hobby>();

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM student_hobby_P INNER JOIN hobby_P ON hobbyhCode = hCode where studentmail ='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Hobby h = new Hobby();
                    h.Hcode = Convert.ToInt32(dr["hCode"]);
                    h.Hname = (string)(dr["hName"]);
                    h.Hicon = (string)(dr["hIcon"]);
                    userHList.Add(h);
                }
                return userHList;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<Pleasure> GetAllPleasures()
        {
            SqlConnection con = null;
            List<Pleasure> PList = new List<Pleasure>();

            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM pleasure_P";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Pleasure p = new Pleasure();
                    p.Pcode = Convert.ToInt32(dr["pCode"]);
                    p.Pname = (string)(dr["pName"]);
                    p.Picon = (string)(dr["pIcon"]);
                    PList.Add(p);
                }
                return PList;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Hobby> GetAllHoddies()
        {
            SqlConnection con = null;
            List<Hobby> HList = new List<Hobby>();

            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM hobby_P";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                { 
                    Hobby h = new Hobby();
                    h.Hcode = Convert.ToInt32(dr["hCode"]);
                    h.Hname = (string)(dr["hName"]);
                    h.Hicon = (string)(dr["hIcon"]);
                    HList.Add(h);
                }
                return HList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public int InsertStudent(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = BuildInsertStudentCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildInsertStudentCommand(Student student)
        {
            String command;
            StringBuilder sb = new StringBuilder();
            String prefix = "INSERT INTO Student_P" + "(mail, password, firstName, lastName, dateOfBirth, departmentCode, studyingYear, homeTown, adrressStudying, personalStatus, isAvailableCar, intrestedInCarpool, photo, gender, registrationDate, active) ";
            sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}','{8}', '{9}','{10}', '{11}','{12}', '{13}','{14}', '{15}')", student.Mail, student.Password, student.Fname, student.Lname, student.DateOfBirth.ToString("yyyy-MM-dd H:mm:ss"), student.Dep.DepartmentCode, student.StudyingYear, student.HomeTown.Name, student.AddressStudying.Name, student.PersonalStatus, student.IsAvailableCar, student.IntrestedInCarPool, student.Photo, student.Gender, student.RegistrationDate.ToString("yyyy-MM-dd H:mm:ss"), true);
            command = prefix + sb.ToString();
            return command;
        }

        public int InsertStudentPleasures(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildInsertStudentPleasuresCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildInsertStudentPleasuresCommand(Student student)
        {
            String prefix = "";
            for (int i = 0; i < student.Plist.Count; i++)
            {
                prefix += "INSERT INTO student_pleasure_P (studentmail, pleasurepCode) Values('" + student.Mail + "'," + student.Plist[i].Pcode + ") ";
            }
            return prefix;
        }

        public int InsertStudentHobbies(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildInsertStudentHobbiesCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildInsertStudentHobbiesCommand(Student student)
        {
            String prefix = "";
            foreach (var hobby in student.Hlist)
            {
                prefix += "INSERT INTO student_hobby_P (studentmail, hobbyhCode) Values('" + student.Mail + "'," + hobby.Hcode + ") ";

            }
            return prefix;
        }
       
        public int UpdateStudentPtofile(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildUpdateStudentProfileCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildUpdateStudentProfileCommand(Student student)
        {
            int carPool = student.IntrestedInCarPool == true ? 1 : 0;
            int withCar = student.IsAvailableCar == true ? 1 : 0;
            String prefix = "UPDATE[dbo].[student_P] SET ";
            prefix += "[photo] = '" + student.Photo + "', [gender] = '" + student.Gender +"', ";
            prefix += "[homeTown] = '" + student.HomeTown.Name + "' , [adrressStudying] = '" + student.AddressStudying.Name + "'";
            prefix += " , [personalStatus] = '" + student.PersonalStatus + "' , [isAvailableCar] = " + withCar + " , [intrestedInCarPool] = " + carPool;
            prefix += " WHERE [mail] = '"+ student.Mail +"'";
            return prefix;
        }

        public int UpdateStudentPleasures(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildUpdateStudentPleasuresCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildUpdateStudentPleasuresCommand(Student student)
        {
            String prefix = "DELETE FROM [dbo].[student_pleasure_P] where [studentmail] = '" + student.Mail + "' ";
            for (int i = 0; i < student.Plist.Count; i++)
            {
                prefix += "INSERT INTO student_pleasure_P (studentmail, pleasurepCode) Values('" + student.Mail + "'," + student.Plist[i].Pcode + ") ";
            }
            return prefix;
        }

        public int UpdateStudentHobbies(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildUpdateStudentHobbiesCommand(student);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildUpdateStudentHobbiesCommand(Student student)
        {
            String prefix = "DELETE FROM [dbo].[student_hobby_P] where [studentmail] = '" + student.Mail + "' ";
            foreach (var hobby in student.Hlist)
            {
                prefix += "INSERT INTO student_hobby_P (studentmail, hobbyhCode) Values('" + student.Mail + "'," + hobby.Hcode + ") ";

            }
            return prefix;
        }

        public int InsertFavorite(StudentFavorites sf)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildInsertFavoriteCommand(sf);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }
        //הבאה של כל היוזרים עם אחוז התאמה
        public List<Student> GetStudentsWithRecommend(string mail)
        {
            SqlConnection con = null;
            List<Student> studList = new List<Student>();

            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM student_P where mail<> '" + mail + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Student stud = new Student();
                    stud.Mail = (string)dr["mail"];
                    stud.Password = (string)dr["password"];
                    stud.Fname = (string)(dr["firstName"]);
                    stud.Lname = (string)(dr["lastName"]);
                    stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
                    stud.Dep = getStudDep(Convert.ToInt32(dr["departmentCode"]));
                    stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);
                    stud.HomeTown = getResidenceH((string)(dr["homeTown"]));
                    stud.AddressStudying = getResidenceS((string)(dr["adrressStudying"]));
                    stud.PersonalStatus = (string)(dr["personalStatus"]);
                    stud.IsAvailableCar = Convert.ToBoolean(dr["isAvailableCar"]);
                    stud.IntrestedInCarPool = Convert.ToBoolean(dr["intrestedInCarPool"]);
                    stud.Photo = (string)(dr["photo"]);
                    stud.Gender = (string)(dr["gender"]);
                    stud.RegistrationDate = Convert.ToDateTime(dr["registrationDate"]);
                    stud.ActiveStatus = Convert.ToBoolean(dr["active"]);
                    stud.Plist = GetPlistByUser((string)dr["mail"]);
                    stud.Hlist = GetHlistByUser((string)dr["mail"]);
                    stud.Friendslist = GetFriendsListByUser(((string)dr["mail"]));
                    stud.Match = calMatch(mail, stud.Friendslist,stud.DateOfBirth, stud.Dep, stud.StudyingYear, stud.HomeTown, stud.AddressStudying, stud.PersonalStatus, stud.Hlist,stud.Plist);
                    studList.Add(stud);
                }

                var newStudList = studList.OrderByDescending(x => x.Match).ToList();
                return newStudList;

            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        private double calMatch(string mail, List<string> friendsList,DateTime dateOfBirth, Department dep, int studyingYear, Residence homeTown, Residence addressStudying, string personalStatus, List<Hobby> hlist, List<Pleasure> plist)
        {
            SqlConnection con = null;
            double match=0;
            int countP = 0;
            int countH = 0;
            int countFriends = 0;
            double xHome = 0;
            double yHome = 0;
            double xCurrent = 0;
            double yCurrent = 0;
            List<int> hlistCode = new List<int>();
            List<int> plistCode = new List<int>();
            try
            {
                con = connect("DBConnectionString"); 

                String selectSTR = "SELECT * FROM student_P where mail='" + mail + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Student stud = new Student();
                    stud.DateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]);
                    stud.Dep = getStudDep(Convert.ToInt32(dr["departmentCode"]));
                    stud.StudyingYear = Convert.ToInt32(dr["studyingYear"]);
                    stud.HomeTown = getResidenceH((string)(dr["homeTown"]));
                    stud.AddressStudying = getResidenceS((string)(dr["adrressStudying"]));
                    stud.PersonalStatus = (string)(dr["personalStatus"]);
                    stud.Plist = GetPlistByUser((string)dr["mail"]);
                    stud.Hlist = GetHlistByUser((string)dr["mail"]);
                    stud.Friendslist = GetFriendsListByUser(((string)dr["mail"]));

                    if (stud.Dep.DepartmentCode == dep.DepartmentCode)
                        match += 5;
                    if (stud.StudyingYear == studyingYear)
                        match += 10;
                    if (stud.PersonalStatus == personalStatus)
                        match += 5;
                    
                    for (int i = 0; i < plist.Count; i++)
                    { plistCode.Add(plist[i].Pcode); }
                    for (int i = 0; i < stud.Plist.Count; i++)
                    {
                        if (plistCode.Contains(stud.Plist[i].Pcode))
                            countP++;
                    }
                    if(stud.Plist.Count !=0 && countP != 0) 
                        match += 20 * (Convert.ToDouble(countP) / Convert.ToDouble(stud.Plist.Count));
                    
                    for (int i = 0; i < hlist.Count; i++)
                    {hlistCode.Add(hlist[i].Hcode);}
                    for (int i = 0; i < stud.Hlist.Count; i++)
                    {
                        if (hlistCode.Contains(stud.Hlist[i].Hcode))
                            countH++;
                    }
                    if (stud.Hlist.Count != 0&&countH!=0)
                        match += 20 * (Convert.ToDouble(countH) / Convert.ToDouble(stud.Hlist.Count));

                    if (Math.Abs(stud.DateOfBirth.Year - dateOfBirth.Year) <= 3 )
                        match += 5;

                    xHome = (stud.HomeTown.X / 1000) - (homeTown.X / 1000);
                    yHome = (stud.HomeTown.Y / 1000) - (homeTown.Y / 1000);
                    if (Math.Sqrt(Math.Pow(xHome, 2) + Math.Pow(yHome, 2)) < 15)
                        match += 20;

                    xCurrent = (stud.AddressStudying.X / 1000) - (addressStudying.X / 1000);
                    yCurrent = (stud.AddressStudying.Y / 1000) - (addressStudying.Y / 1000);
                    if (Math.Sqrt(Math.Pow(xCurrent, 2) + Math.Pow(yCurrent, 2)) < 15)
                        match += 20;

                    for (int i = 0; i < stud.Friendslist.Count; i++)
                    {
                        if (friendsList.Contains(stud.Friendslist[i]))
                            countFriends++;
                    }
                    if (stud.Friendslist.Count != 0 && countFriends != 0)
                    {
                        if (countFriends >5)
                            countFriends = 5;
                        match += 15 * (Convert.ToDouble(countFriends) / 5);
                    }
                }

                if (match > 100)
                    return 100;
                else
                    return Math.Round(match);

            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }


        private String BuildInsertFavoriteCommand(StudentFavorites sf)
        {
            String command;
            StringBuilder sb = new StringBuilder();
            String prefix = "INSERT INTO student_favorites_P" + "(Student1Mail, student2Mail) ";
            sb.AppendFormat("Values('{0}', '{1}')", sf.Student1mail, sf.Student2mail);
            command = prefix + sb.ToString();
            return command;
        }
        public int DeleteFavorite(StudentFavorites sf)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            String cStr = BuildDeleteFavoriteCommand(sf);
            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                writeToLog(ex);
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }
        private String BuildDeleteFavoriteCommand(StudentFavorites sf)
        {
            String command = "DELETE from [dbo].[student_favorites_P] where [Student1Mail] = '" + sf.Student1mail +"' and [student2Mail] = '" +  sf.Student2mail +"'";
            return command;
        }

        public void writeToLog(Exception ex)
        {
            // Create a string array with the lines of text
            string line = ex.Message;

            // Set a variable to the Documents path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "Exceptions.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Exceptions.txt"),true))
            {
                    outputFile.WriteLine(line);
            }
        }

    }
}
