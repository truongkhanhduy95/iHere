using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;
using System;

namespace Xamarin.Core.Models
{
	/// <summary>
	/// Abstract to use SQLite ORM.
	/// Sub classes don't have to add PrimaryKey, AutoIncrement attributes.
	/// </summary>
	public abstract class BaseEntity : IEntity
	{
		[JsonIgnore]
		public int Id { get; set; }
		[JsonIgnore]
		[Ignore]
		public virtual bool IsProtected
		{
			get
			{
				return false;
			}
		}
	}

	/// <summary>
	/// Interface to be implemented by all entities.
	/// </summary>
	public interface IEntity
	{
		int Id { get; set; }
		bool IsProtected { get; }
	}

	public abstract class BaseAutoIncrease : IEntity
	{
		[PrimaryKey, AutoIncrement]
		[JsonIgnore]
		public int Id
		{
			get;
			set;
		}

		[JsonIgnore]
		[Ignore]
		public virtual bool IsProtected
		{
			get
			{
				return false;
			}
		}
	}
}