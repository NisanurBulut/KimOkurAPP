export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPage: number;
}

export class PaginatedResult<T> {
  // mesaj listesindede kullanılacak
  result: T;
  pagination: Pagination;
}
