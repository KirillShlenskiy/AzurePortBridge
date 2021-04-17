//using System;
//using System.ServiceModel;

//namespace Microsoft.Samples.ServiceBus.Connections
//{
//    [ServiceContract(Namespace = "n:",
//      Name = "idx", CallbackContract = typeof(IDataExchange),
//      SessionMode = SessionMode.Required)]
//    public interface IDataExchange
//    {
//        [OperationContract(Action = "c",
//          IsOneWay = true, IsInitiating = true)]
//        void Connect(string i);
//        [OperationContract(Action = "w", IsOneWay = true)]
//        void Write(TransferBuffer d);
//        [OperationContract(Action = "d",
//          IsOneWay = true, IsTerminating = true)]
//        void Disconnect();
//    }
//    public interface IDataExchangeChannel :
//      IDataExchange, IClientChannel
//    { }
//}