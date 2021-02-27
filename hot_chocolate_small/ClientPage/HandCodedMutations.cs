using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.ClientPage.Entities;
using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage {
  public partial class Mutations {
    /// <summary>
    /// Set a particular Company Entity as the "Primary" - and sets all others to false.
    /// </summary>
    public IEnumerable<CompanyEntity> SetCompanyEntityAsPrimary(
      string companyEntityId,
      [Service] IRepository repository) {

        Company? company = repository.GetCompanies()
          .SingleOrDefault(x => x.Entities != null && x.Entities.Any(x => x.Id == companyEntityId));

        if (company == null)
          throw new Exception("No company for CompanyEntity with id " + companyEntityId);

        foreach (CompanyEntity entity in company.Entities)
          entity.IsPrimary = entity.Id == companyEntityId;

        return company.Entities;
    }
  }
}