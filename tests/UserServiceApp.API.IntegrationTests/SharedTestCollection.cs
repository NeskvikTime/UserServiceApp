﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceApp.API.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<ApplicationApiFactory>
{

}
