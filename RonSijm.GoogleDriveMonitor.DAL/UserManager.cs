using System;
using System.Collections.Generic;

using Google.Apis.Drive.v3.Data;

namespace RonSijm.GoogleDriveMonitor.DAL;

public static class UserManager
{
    private static Dictionary<string, User> _users;

    public static void FixUsers(File file)
    {
        if (file == null)
        {
            return;
        }

        if (file.Owners != null)
        {
            for (var index = 0; index < file.Owners.Count; index++)
            {
                var user = file.Owners[index];
                var userIdentifier = user.GetIdentifier();
                var existingUser = _users.TryGetValue(userIdentifier, out var existingOwner);

                if (existingUser)
                {
                    file.Owners[index] = existingOwner;
                }
                else
                {
                    _users[userIdentifier] = user;
                }
            }
        }

        FixField(() => file.LastModifyingUser, x => file.LastModifyingUser = x);
        FixField(() => file.SharingUser, x => file.SharingUser = x);
        FixField(() => file.TrashingUser, x => file.TrashingUser = x);
    }
        
    public static void FixUsers(Comment comment)
    {
        FixField(() => comment.Author, x => comment.Author = x);

        if (comment.Replies != null)
        {
            foreach (var reply in comment.Replies)
            {
                FixField(() => reply.Author, x => reply.Author = x);
            }
        }
    }

    private static void FixField(Func<User> getUser, Action<User> setUser)
    {
        var user = getUser();

        if (user == null)
        {
            return;
        }

        var userIdentifier = user.GetIdentifier();
        var existingUser = _users.TryGetValue(userIdentifier, out var existingOwner);

        if (existingUser)
        {
            setUser(existingOwner);
        }
        else
        {
            _users[userIdentifier] = user;
        }
    }

    public static void Init(Dictionary<string, User> users)
    {
        _users = users;
    }
}