﻿using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IPersonRepository
    {
        Person AddUser(Person user);

        Person? DeleteUser(int userId);

        IEnumerable<Person> GetAllUsers();

        Person? GetByID(int id);

        Person UpdateUser(Person user);

    }
}