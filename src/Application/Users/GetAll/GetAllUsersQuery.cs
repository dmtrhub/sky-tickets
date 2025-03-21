﻿using Application.Abstractions.Messaging;

namespace Application.Users.GetAll;

public sealed record GetAllUsersQuery : IQuery<List<UserResponse>>;