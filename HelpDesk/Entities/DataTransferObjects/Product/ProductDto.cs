﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Entities.DataTransferObjects.Product
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
    }
}
