CREATE TABLE "Posts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY);

CREATE TABLE "Tags" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Tags" PRIMARY KEY);

CREATE TABLE "PostsToTagsJoinTable" (
    "PostsId" INTEGER NOT NULL,
    "TagsId" INTEGER NOT NULL,
    CONSTRAINT "PK_PostsToTagsJoinTable" PRIMARY KEY ("PostsId", "TagsId"),
    CONSTRAINT "FK_PostsToTagsJoinTable_Posts_PostsId" FOREIGN KEY ("PostsId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PostsToTagsJoinTable_Tags_TagsId" FOREIGN KEY ("TagsId") REFERENCES "Tags" ("Id") ON DELETE CASCADE);

--CREATE TABLE "PostTag" (
--    "PostsId" INTEGER NOT NULL,
--    "TagsId" INTEGER NOT NULL,
--    CONSTRAINT "PK_PostTag" PRIMARY KEY ("PostsId", "TagsId"),
--    CONSTRAINT "FK_PostTag_Posts_PostsId" FOREIGN KEY ("PostsId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
--    CONSTRAINT "FK_PostTag_Tags_TagsId" FOREIGN KEY ("TagsId") REFERENCES "Tags" ("Id") ON DELETE CASCADE);