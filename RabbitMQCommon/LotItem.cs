﻿using System;

namespace RabbitMQCommon
{
    public class LotItem
    {

        public string User { get; set; }

        public long Contract { get; set; }

        public DateTime Date { get; set; }

        public int Lot { get; set; }

        public decimal Price { get; set; }

    }
}
