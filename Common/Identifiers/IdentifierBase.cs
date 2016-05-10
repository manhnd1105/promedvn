using NLog;
using System;
using System.Text;

namespace Common.Identifiers
{
	public class IdentifierBase
	{
		private Logger _log;
		#region Properties
		/// <summary>
		/// The Issue ID that can be persisted
		/// </summary>
		public int PersistableID { get; private set; }

		/// <summary>
		/// Id that is displayable
		/// </summary>
		public string DisplayableIdentifier { get; private set; }

		public string PrependedString { get; private set; }

		#endregion //Properties

		#region Constructors
		public IdentifierBase(int persistableID)
		{
            _log = new LogFactory().GetCurrentClassLogger();
			PersistableID = persistableID;
			DisplayableIdentifier = VerhoeffCheckDigit.AppendCheckDigit(persistableID).ToString();
		}

		public IdentifierBase(string prependedString, int persistableID)
		{
            _log = new LogFactory().GetCurrentClassLogger();
            PersistableID = persistableID;
			PrependedString = prependedString;
			DisplayableIdentifier = PrependedString + VerhoeffCheckDigit.AppendCheckDigit(persistableID).ToString();
		}

		//[Obsolete("Use Identifier(string prependedString, string displayableID) for better error checking")]
		public IdentifierBase(string displayableID) : this(null, displayableID)
		{

		}

		public IdentifierBase(string prependedString, string displayableID)
		{
            _log = new LogFactory().GetCurrentClassLogger();
            DisplayableIdentifier = displayableID;

			PrependedString = ParseStartingString(displayableID);

			if (
				!String.IsNullOrWhiteSpace(prependedString)
				&&
				!String.IsNullOrWhiteSpace(PrependedString)
				&&
				!String.Equals(prependedString, PrependedString, StringComparison.InvariantCultureIgnoreCase)
				)
			{
				throw new ArgumentException(
					String.Format(
						"The Identifier is not valid because the prepending string was not correct - DisplayableIdentifier: {0}; Expected Prepended String: {1};",
						DisplayableIdentifier,
						prependedString
					));
			}

			if (!String.IsNullOrEmpty(displayableID) && !String.IsNullOrEmpty(PrependedString))
			{
				displayableID = displayableID.Remove(0, PrependedString.Length);
			}

			if (!IsValid(displayableID))
			{
				throw new ArgumentException(
					String.Format(
						"The Identifier is not valid - DisplayableIdentifier: {0};",
						DisplayableIdentifier
					));
			}

			//Get the raw id
			//Take out the last digit
			PersistableID = Int32.Parse(displayableID.Substring(0, displayableID.Length - 1));
		}

		private static string ParseStartingString(string displayableID)
		{
			StringBuilder prependedString = new StringBuilder();

			foreach (char charValue in displayableID)
			{
				if (char.IsDigit(charValue))
				{
					//We're done -- break out
					break;
				}

				prependedString.Append(charValue);
			}

			return prependedString.ToString();
		}

		#endregion

		#region Methods
		public bool IsValid(string calculatedID)
		{
			try
			{
				if (string.IsNullOrEmpty(calculatedID))
				{
					return false;
				}
				string startingString = ParseStartingString(calculatedID);

				calculatedID = calculatedID.Remove(0, startingString.Length);

				return VerhoeffCheckDigit.Check(calculatedID);
			}
			catch (FormatException ex)
			{
				_log.Error(ex, "Parsing the string {"+calculatedID+"} for the identifier");
				return false;
			}
		}
		public override string ToString()
		{
			return DisplayableIdentifier.ToString();
		}
		#endregion
	}
}
