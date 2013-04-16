using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DocumentSessionManager
{
  public class XmlSessionRepository : ISessionRepository
  {
    private const string TAG_DOCUMENT = "Document";
    private const string TAG_DOCUMENTS = "Documents";
    private const string ATTRIBUTE_DOC_PATH = "path";
    private const string ATTRIBUTE_DOC_TYPE = "type";
    private const string TAG_SESSION = "Session";
    private const string ATTRIBUTE_SESSION_NAME = "name";
    private const string TAG_SESSIONS = "Sessions";
    private const string TAG_CURRENT_SESSION = "CurrentSession";
    private const string ATTRIBUTE_CURRENT_SESSION_NAME = "name";
    private const string TAG_SESSIONS_SETTINGS = "SessionsSettings";

    public XmlSessionRepository()
    {
      if (!File.Exists(FilePath))
        createFile();
    }

    private const string FILE = "DsmSessions.xml";

    private static string _filePath;
    public static string FilePath
    {
      get
      {
        if (string.IsNullOrEmpty(_filePath))
        {
          string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Remove(0, "file:///".Length));
          _filePath = Path.Combine(currentPath, FILE);
        }

        return _filePath;
      }
    }

    public IEnumerable<Session> GetAllSessions()
    {
      var repositoryFile = getRepositoryFile();

      var sessionsList = new List<Session>();

      var xmlSessionsNode = getSecondLevelNode(repositoryFile, TAG_SESSIONS);

      foreach (XmlNode sessionNode in xmlSessionsNode)
      {
        var session = new Session(sessionNode.Attributes[ATTRIBUTE_SESSION_NAME].Value);

        foreach (XmlNode documentNode in sessionNode.ChildNodes[0].ChildNodes)
        {
          var document = new SessionDocument(documentNode.Attributes[ATTRIBUTE_DOC_PATH].Value, 
            (DocumentType)Enum.Parse(typeof(DocumentType), documentNode.Attributes[ATTRIBUTE_DOC_TYPE].Value));
          session.AddDocument(document);
        }

        sessionsList.Add(session);
      }

      return sessionsList.AsEnumerable();
    }

    public string GetCurrentSessionName()
    {
      var repositoryFile = getRepositoryFile();

      var xmlCurrentSessionNode = getSecondLevelNode(repositoryFile, TAG_CURRENT_SESSION);
      return xmlCurrentSessionNode.Attributes[ATTRIBUTE_CURRENT_SESSION_NAME].Value;
    }

    private static XmlNode getSecondLevelNode(XmlDocument repositoryFile, string nodeName)
    {
      foreach (XmlNode node in repositoryFile.ChildNodes)
      {
        if (node.Name.Equals(TAG_SESSIONS_SETTINGS))
        {
          foreach (XmlNode node2 in node.ChildNodes)
          {
            if (node2.Name.Equals(nodeName))
            {
              return node2;
            }
          }
        }
      }

      throw new Exception(String.Format("{0} node not found", nodeName));
    }

    private static bool match(XmlNode node, Session session)
    {
      return node.Name.Equals(TAG_SESSION) && 
        node.Attributes[ATTRIBUTE_SESSION_NAME].Value.Equals(session.Name);
    }

    public void Persist(IList<object> savedOrUpdated, IList<object> removed)
    {
      if ((savedOrUpdated.Count == 0) && (removed.Count == 0))
        return;

      var repositoryFile = getRepositoryFile();

      XmlNode rootSessions = getSecondLevelNode(repositoryFile, TAG_SESSIONS);

      foreach (var obj in removed)
      {
        var session = obj as Session;
        if (session != null)
        {
          for (int i = rootSessions.ChildNodes.Count - 1; i >= 0; i--)
          {
            if (match(rootSessions.ChildNodes[i], session))
              rootSessions.RemoveChild(rootSessions.ChildNodes[i]);
          }
        }
      }

      foreach (var obj in savedOrUpdated)
      {
        var session = obj as Session;
        if (session != null)
        {
          bool found = false;
          for (int i = 0; i < rootSessions.ChildNodes.Count; i++)
          {
            if (match(rootSessions.ChildNodes[i], session))
            {
              found = true;

              var sessionNode = rootSessions.ChildNodes[i];
              fillDocuments(session, sessionNode);

              break;
            }
          }

          if (!found)
            addSessionNode(rootSessions, session);
        }
      }

      foreach (var obj in savedOrUpdated)
      {
        var currentSessionInfo = obj as CurrentSessionInfo;
        if (currentSessionInfo != null)
        {
          XmlNode currentSessionNode = getSecondLevelNode(repositoryFile, TAG_CURRENT_SESSION);
          currentSessionNode.Attributes[ATTRIBUTE_CURRENT_SESSION_NAME].Value = currentSessionInfo.CurrentSessionName;
        }
      }

      repositoryFile.Save(FilePath);
    }

    private XmlDocument getRepositoryFile()
    {
      var repositoryFile = new XmlDocument();
      repositoryFile.Load(FilePath);
      return repositoryFile;
    }

    private void addSessionNode(XmlNode rootSessions, Session session)
    {
      var xmlSessionNode = rootSessions.OwnerDocument.CreateElement("", TAG_SESSION, "");
      xmlSessionNode.SetAttributeNode(ATTRIBUTE_SESSION_NAME, "").Value = session.Name;
      rootSessions.AppendChild(xmlSessionNode);

      fillDocuments(session, xmlSessionNode);
    }

    private void fillDocuments(Session session, XmlNode sessionNode)
    {
      XmlNode xmlDocumentsNode;
      if (sessionNode.ChildNodes.Count == 0)
      {
        xmlDocumentsNode = sessionNode.OwnerDocument.CreateElement("", TAG_DOCUMENTS, "");
        sessionNode.AppendChild(xmlDocumentsNode);
      }
      else
        xmlDocumentsNode = sessionNode.FirstChild;

      xmlDocumentsNode.RemoveAll();

      foreach (var d in session.GetDocuments())
        addDocumentNode(xmlDocumentsNode, d);
    }

    private static void addDocumentNode(XmlNode xmlDocumentsNode, SessionDocument document)
    {
      var xmlDocNode = xmlDocumentsNode.OwnerDocument.CreateElement("", TAG_DOCUMENT, "");
      xmlDocNode.SetAttributeNode(ATTRIBUTE_DOC_PATH, "").Value = document.Path;
      xmlDocNode.SetAttributeNode(ATTRIBUTE_DOC_TYPE, "").Value = document.Type.ToString();
      xmlDocumentsNode.AppendChild(xmlDocNode);
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return new UnitOfWork(this);
    }

    private static void createFile()
    {
      var repositoryFile = new XmlDocument();

      var xmlDecl = repositoryFile.CreateNode(XmlNodeType.XmlDeclaration, "", "");
      repositoryFile.AppendChild(xmlDecl);

      var xmlSessionsSettingsNode = repositoryFile.CreateElement("", TAG_SESSIONS_SETTINGS, "");
      repositoryFile.AppendChild(xmlSessionsSettingsNode);

      var xmlCurrentSessionNode = repositoryFile.CreateElement("", TAG_CURRENT_SESSION, "");
      xmlCurrentSessionNode.SetAttributeNode(ATTRIBUTE_CURRENT_SESSION_NAME, "").Value = "";
      xmlSessionsSettingsNode.AppendChild(xmlCurrentSessionNode);

      var xmlSessionsNode = repositoryFile.CreateElement("", TAG_SESSIONS, "");
      xmlSessionsSettingsNode.AppendChild(xmlSessionsNode);

      repositoryFile.Save(FilePath);
    }

  }
}