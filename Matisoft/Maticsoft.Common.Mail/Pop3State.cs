using System;
namespace Maticsoft.Common.Mail
{
	[Flags]
	public enum Pop3State
	{
		Unknown = 0,
		Authorization = 1,
		Transaction = 2,
		Update = 4
	}
}
