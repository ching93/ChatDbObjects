using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace ChatDbObjects
{
    public enum dbAction { NONE, ADD_USER, VERIFY_USER, ADD_CHAT, ADD_MESSAGE, ADD_TASK, ADD_PHOTO };
    public enum dbSelectAction {NONE, SELECT_ALL_USERS, SELECT_USERS_BY_LOGIN, SELECT_ROLES, SELECT_CHAT_TYPES };
    [Serializable]
    public class DbObject
    {
        public DbObject()
        {
        }
        public static byte[] Serialize(DbObject obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            byte[] bytes = stream.ToArray();
            return bytes;
        }
        public static DbObject Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (DbObject)formatter.Deserialize(stream);
        }
        public static DbObject Deserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return (DbObject)formatter.Deserialize(stream);
        }
    }

    [Serializable]
    public class dbRequest: DbObject
    {
        public dbAction action;
        public dbSelectAction selectAction;
        public User currentUser;
        public Entity entity;
        public dbRequest(dbAction action, dbSelectAction selectAction, User user, Entity entity)
        {
            currentUser = user;
            this.action = action;
            this.selectAction = selectAction;
            this.entity = entity;
        }
    }
    [Serializable]
    public class dbResult: DbObject
    {
        public dbAction action;
        public dbSelectAction selectAction;
        public bool isSuccessful;
        public Entity[] objects;
        public String message;
        public dbResult(dbAction action, dbSelectAction selectAction, bool isSuccessful)
        {
            this.isSuccessful = isSuccessful;
            objects = null;
            this.action = action;
            this.selectAction = selectAction;
        }
    }
    [Serializable]
    public abstract class Entity
    {
        public static byte[] Serialize(Entity obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            byte[] bytes = stream.ToArray();
            return bytes;
        }
        public static Entity Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (Entity)formatter.Deserialize(stream);
        }
    }

    [Serializable]
    public class Role : Entity
    {
        public int id;
        public String Name;
    }
    [Serializable]
    public class User : Entity
    {
        public int id;
        public String login;
        public Role role;
        public String password;
        public String firstname;
        public String lastname;
        public String email;
        public DateTime registerTime;
        public int photo_file_id;
    }
    [Serializable]
    public class Exercise : Entity
    {
        public int id;
        public String name;
        public String[] Description;
        public DateTime allocTime;
        public DateTime duration;
        public bool IsDone;
        public User[] executers;
    }
    [Serializable]
    public class ChatType : Entity
    {
        public int id;
        public String name;
    }
    [Serializable]
    public class Message : Entity
    {
        public int id;
        public String[] text;
        public User author;
        public DateTime sendTime;
        public File[] embeddings;
    }
    [Serializable]
    public class Chat : Entity
    {
        public int id;
        public String name;
        public ChatType type;
        public DateTime createTime;
        public Message[] messages;
        public User creator;
        public User[] members;
    }
    [Serializable]
    public class File : Entity
    {
        public int id;
        public byte[] file;
        public bool IsImage;
    }
}
