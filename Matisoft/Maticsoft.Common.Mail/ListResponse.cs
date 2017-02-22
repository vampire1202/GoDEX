using System;
using System.Collections.Generic;
namespace Maticsoft.Common.Mail
{
	internal sealed class ListResponse : Pop3Response
	{
		private List<Pop3ListItem> _items;
		public List<Pop3ListItem> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}
		public int MessageNumber
		{
			get
			{
				return this._items[0].MessageId;
			}
		}
		public long Octets
		{
			get
			{
				return this._items[0].Octets;
			}
		}
		public ListResponse(Pop3Response response, List<Pop3ListItem> items) : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this._items = items;
		}
	}
}
