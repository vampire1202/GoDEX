using System;
namespace Maticsoft.Common.Mime
{
	public static class MediaTypes
	{
		public static readonly string Multipart;
		public static readonly string Mixed;
		public static readonly string Alternative;
		public static readonly string MultipartMixed;
		public static readonly string MultipartAlternative;
		public static readonly string TextPlain;
		public static readonly string TextHtml;
		public static readonly string TextRich;
		public static readonly string TextXml;
		public static readonly string Message;
		public static readonly string Rfc822;
		public static readonly string MessageRfc822;
		public static readonly string Application;
		static MediaTypes()
		{
			MediaTypes.Multipart = "multipart";
			MediaTypes.Mixed = "mixed";
			MediaTypes.Alternative = "alternative";
			MediaTypes.Message = "message";
			MediaTypes.Rfc822 = "rfc822";
			MediaTypes.MultipartMixed = MediaTypes.Multipart + "/" + MediaTypes.Mixed;
			MediaTypes.MultipartAlternative = MediaTypes.Multipart + "/" + MediaTypes.Alternative;
			MediaTypes.MessageRfc822 = MediaTypes.Message + "/" + MediaTypes.Rfc822;
			MediaTypes.TextPlain = "text/plain";
			MediaTypes.TextHtml = "text/html";
			MediaTypes.TextRich = "text/richtext";
			MediaTypes.TextXml = "text/xml";
		}
	}
}
