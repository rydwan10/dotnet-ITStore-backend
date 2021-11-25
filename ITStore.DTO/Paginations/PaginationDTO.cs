﻿namespace ITStore.DTOs.Paginations
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage { get; set; } = 10;
        public readonly int MaxAmount = 50;
        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > MaxAmount) ? MaxAmount : value;
            }
        }
    }
}
