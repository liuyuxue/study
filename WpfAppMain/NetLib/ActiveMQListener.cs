using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;

namespace NetLib
{
    public delegate void MessageRecivedHandler(string message);


    public enum MQType
    {
        Queue, Topic
    }


    public class ActiveMQListener : IDisposable
    {
        private readonly string mqname = null;
        private readonly IConnectionFactory connectionFactory = null;
        private ISession session = null;
        private readonly IConnection connection = null;
        private IMessageConsumer consumer = null;
        private readonly MQType type;
        private readonly string filter;
        private bool isDisposed = false;
        public event MessageRecivedHandler OnMessageReceived;

        public ActiveMQListener( MQType mqtype ,String mqname, string brokerUri, string clientid=null, string filter = null)
        {
            this.mqname = mqname;
            type = mqtype;
            this.filter = filter;
            connectionFactory = new ConnectionFactory(brokerUri);
            connection = connectionFactory.CreateConnection();
           
            if (!string.IsNullOrEmpty(clientid))
                connection.ClientId = clientid;
        }

        public void Listen()
        {

            if (connection.IsStarted)
                connection.Stop();
            connection.Start();
            session = connection.CreateSession();

            ActiveMQDestination queue = null;
            if (type == MQType.Queue)
                queue = new ActiveMQQueue(mqname);
            else
                queue = new ActiveMQTopic(mqname);
            consumer = session.CreateConsumer(queue, filter);
            consumer.Listener += Received;
        }

        public void StartListenDurable()
        {
            if (type != MQType.Topic)
                throw new Exception("Current MQType no support.");
            if (connection.IsStarted)
                connection.Stop();
            connection.Start();
            session = connection.CreateSession();
            ITopic queue = new ActiveMQTopic(mqname);
            consumer = session.CreateDurableConsumer(queue, mqname, filter, false);
            consumer.Listener += Received;
        }

        public void Stop()
        {
            if (connection.IsStarted)
                connection.Stop();
        }

        public void Close()
        {
            if (connection.IsStarted)
                connection.Stop();
            connection.Close();
        }

        private void Received(IMessage message)
        {
            var txtmsg = message as ITextMessage;
            OnMessageReceived?.Invoke(txtmsg.Text);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            if(disposing)
            {
                Close();
                consumer?.Dispose();
                session?.Dispose();
                connection?.Dispose();
                isDisposed = true;
            }
        }


        ~ActiveMQListener()
        {
            Dispose(false);
        }
    }
}
