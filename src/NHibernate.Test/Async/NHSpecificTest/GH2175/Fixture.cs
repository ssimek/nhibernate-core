﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH2175
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : TestCaseMappingByCode
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Concept>(m =>
			{
				m.Lazy(false);
				m.Id(c => c.Id, id =>
				{
					id.Column("concept_id");
					id.Generator(Generators.Native);
				});
				m.Set(c => c.Mappings,
					  collection =>
					  {
						  collection.Fetch(CollectionFetchMode.Subselect);
						  collection.Lazy(CollectionLazy.NoLazy);
						  collection.Table("concept_mapping");
						  collection.Key(key =>
						  {
							  key.Column("concept_id");
						  });
					  },
					  element =>
					  {
						  element.Component(component =>
						  {
							  component.Property(c => c.Relationship, p => p.Column("relationship"));
							  component.ManyToOne(c => c.Code, manyToOne =>
							  {
								  manyToOne.Column("code_id");
								  manyToOne.Fetch(FetchKind.Join);
								  manyToOne.Cascade(NHibernate.Mapping.ByCode.Cascade.None);
							  });
						  });
					  });
			});

			mapper.Class<ConceptCode>(m =>
			{
				m.Table("concept_code");
				m.Lazy(false);

				m.Id(c => c.Id, id =>
				{
					id.Column("code_id");
					id.Generator(Generators.Native);
				});

				m.Property(c => c.CodeSource, p =>
				{
					p.Column("source");
				});
				m.Property(c => c.Value, p =>
				{
					p.Column("`value`");
				});
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public async Task SubcriteriaOnNestedComponentAsync()
		{
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var concepts = new List<Concept>();
				for (var i = 1; i < 10; i++)
				{
					var concept = new Concept { DisplayName = $"Concept{i}" };
					concept.Mappings.Add(new ConceptCodeMapping(ConceptCodeRelationship.SameAs,
						await (CreatePersistentCodeAsync(session, "src-x", "C" + i))));
					concept.Mappings.Add(new ConceptCodeMapping(ConceptCodeRelationship.SameAs,
						await (CreatePersistentCodeAsync(session, "src-y", "CX" + (0x1000 + i).ToString("X")))));
					concepts.Add(concept);
					await (session.SaveAsync(concept));
				}

				var criteria = session.CreateCriteria<Concept>()
					.CreateAlias(nameof(Concept.Mappings), "cm")
					.CreateCriteria("cm." + nameof(ConceptCodeMapping.Code), "code")
					.Add(Restrictions.Eq(nameof(ConceptCode.CodeSource), "src-y"))
					.Add(Restrictions.In(nameof(ConceptCode.Value), new object[] { "CX1001", "CX1003", "CX1008" }));

				var persistentConcepts = await (criteria.ListAsync<Concept>());
				Assert.That(persistentConcepts, Is.EquivalentTo(new[] { concepts[0], concepts[2], concepts[7] }));
			}
		}

		private async Task<ConceptCode> CreatePersistentCodeAsync(ISession session, string codeSource, string value, CancellationToken cancellationToken = default(CancellationToken))
		{
			var code = new ConceptCode(codeSource, value);
			await (session.SaveAsync(code, cancellationToken));
			return code;
		}
	}
}
