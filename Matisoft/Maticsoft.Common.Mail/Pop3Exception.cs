using System;
using System.Runtime.Serialization;
namespace Maticsoft.Common.Mail
{
	[Serializable]
	public class Pop3Exception : Exception
	{
		public Pop3Exception()
		{
		}
		public Pop3Exception(string message) : base(message)
		{
		}
		public Pop3Exception(string message, Exception inner) : base(message, inner)
		{
		}
		protected Pop3Exception(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
