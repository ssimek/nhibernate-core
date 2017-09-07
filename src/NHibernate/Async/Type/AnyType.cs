﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Collections.Generic;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class AnyType : AbstractType, IAbstractComponentType, IAssociationType
	{

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("object is a multicolumn type");
		}

		public override async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await (ResolveAnyAsync((string)await (metaType.NullSafeGetAsync(rs, names[0], session, owner, cancellationToken)).ConfigureAwait(false), 
				await (identifierType.NullSafeGetAsync(rs, names[1], session, owner, cancellationToken)).ConfigureAwait(false), session, cancellationToken)).ConfigureAwait(false);
		}

		public override async Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string entityName = (string)await (metaType.NullSafeGetAsync(rs, names[0], session, owner, cancellationToken)).ConfigureAwait(false);
			object id = await (identifierType.NullSafeGetAsync(rs, names[1], session, owner, cancellationToken)).ConfigureAwait(false);
			return new ObjectTypeCacheEntry(entityName, id);
		}

		public override Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			try
			{
				ObjectTypeCacheEntry holder = (ObjectTypeCacheEntry) value;
				return ResolveAnyAsync(holder.entityName, holder.id, session, cancellationToken);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public override Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("any mappings may not form part of a property-ref");
		}

		public override async Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			object id;
			string entityName;
			if (value == null)
			{
				id = null;
				entityName = null;
			}
			else
			{
				entityName = session.BestGuessEntityName(value);
				id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, value, session, cancellationToken)).ConfigureAwait(false);
			}

			// metaType is assumed to be single-column type
			if (settable == null || settable[0])
			{
				await (metaType.NullSafeSetAsync(st, entityName, index, session, cancellationToken)).ConfigureAwait(false);
			}
			if (settable == null)
			{
				await (identifierType.NullSafeSetAsync(st, id, index + 1, session, cancellationToken)).ConfigureAwait(false);
			}
			else
			{
				bool[] idsettable = new bool[settable.Length - 1];
				Array.Copy(settable, 1, idsettable, 0, idsettable.Length);
				await (identifierType.NullSafeSetAsync(st, id, index + 1, idsettable, session, cancellationToken)).ConfigureAwait(false);
			}
		}

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return NullSafeSetAsync(st, value, index, null, session, cancellationToken);
		}

		public override Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			try
			{
				ObjectTypeCacheEntry e = cached as ObjectTypeCacheEntry;
				return (e == null) ? Task.FromResult<object>(null ): session.InternalLoadAsync(e.entityName, e.id, false, false, cancellationToken);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return value == null ? null : 
				new ObjectTypeCacheEntry(session.BestGuessEntityName(value), 
				await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(session.BestGuessEntityName(value), value, session, cancellationToken)).ConfigureAwait(false));
		}

		public override async Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner,
									   IDictionary copiedAlready, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (original == null)
			{
				return null;
			}
			else
			{
				string entityName = session.BestGuessEntityName(original);
				object id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, original, session, cancellationToken)).ConfigureAwait(false);
				return await (session.InternalLoadAsync(entityName, id, false, false, cancellationToken)).ConfigureAwait(false);
			}
		}

		public Task<object> GetPropertyValueAsync(Object component, int i, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			try
			{
				return i == 0 ? Task.FromResult<object>(session.BestGuessEntityName(component) ): IdAsync(component, session, cancellationToken);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public async Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return new object[] { session.BestGuessEntityName(component), await (IdAsync(component, session, cancellationToken)).ConfigureAwait(false) };
		}

		private static async Task<object> IdAsync(object component, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(session.BestGuessEntityName(component), component, session, cancellationToken)).ConfigureAwait(false);
			}
			catch (TransientObjectException)
			{
				return null;
			}
		}

		public override Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<bool>(cancellationToken);
			}
			//TODO!!!
			return IsDirtyAsync(old, current, session, cancellationToken);
		}

		public override async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (current == null)
				return old != null;
			if (old == null)
				return current != null;
			ObjectTypeCacheEntry holder = (ObjectTypeCacheEntry)old;
			bool[] idcheckable = new bool[checkable.Length - 1];
			Array.Copy(checkable, 1, idcheckable, 0, idcheckable.Length);
			return (checkable[0] && !holder.entityName.Equals(session.BestGuessEntityName(current))) || 
				await (identifierType.IsModifiedAsync(holder.id, await (IdAsync(current, session, cancellationToken)).ConfigureAwait(false), idcheckable, session, cancellationToken)).ConfigureAwait(false);
		}

		private Task<object> ResolveAnyAsync(string entityName, object id, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return entityName == null || id == null ? Task.FromResult<object>(null ): session.InternalLoadAsync(entityName, id, false, false, cancellationToken);
		}

	}
}