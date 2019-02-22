using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public class ActiveMQPublisher : IDisposable
    {

        private readonly string mqname = null;
        private readonly IConnectionFactory connectionFactory = null;
        private readonly IConnection connection = null;
        private readonly ISession session = null;
        private readonly IMessageProducer producer = null;
        private bool isDisposed = false;

        public ActiveMQPublisher(MQType mqtype , string mqname, string brokerUri )
        {
            this.mqname = mqname;
            connectionFactory = new ConnectionFactory(brokerUri);
            connection = connectionFactory.CreateConnection();
            connection.Start();
            session = connection.CreateSession();
            ActiveMQDestination queue = null;
            if (mqtype == MQType.Queue)
                queue = new ActiveMQQueue(mqname);
            else
                queue = new ActiveMQTopic(mqname);
            producer = session.CreateProducer(queue);
        }

        public void SendMessage(string msg)
        {
            if (!isDisposed)
            {
                ITextMessage txtmessage = session.CreateTextMessage(msg);
                txtmessage.NMSDeliveryMode = MsgDeliveryMode.NonPersistent;
                producer.Send(txtmessage);
            }
            else
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            if (disposing)
            {
                producer.Dispose();
                session.Dispose();
                connection.Dispose();
                isDisposed = true;
            }
        }
        ~ActiveMQPublisher()
        {
            Dispose(false);
        }
    }
}
