﻿using sgs.Models.Domain;
using System.Collections.Generic;

namespace sgs.Models.Repository
{
    public interface ISuburb
    {
        List<Suburb> GetAllBySection(string section);
    }
}