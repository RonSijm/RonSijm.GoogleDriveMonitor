# About

This project lets you create a backup of all meta data from a google drive, and saves all the properties in a sqlite database.

This can be used to easier search through your files, file history, file changes, comments and replies.

# Getting Started

## Creating project and credentials in Google.

This part describes how to create a project and credentials in [[Google]].
Note: It doesn't really matter in which account you create these credentials. Some corporate google accounts have disabled creating projects and API based client credentials. But you can just create a project from your personal google account, and then later login from your corporate google account into this project.

Create a new Google Project:
https://console.cloud.google.com/projectcreate

Enable the Google Drive API:
https://console.cloud.google.com/apis/api/drive.googleapis.com

Create credentials for the API:
https://console.cloud.google.com/apis/credentials/wizard?api=drive.googleapis.com

Download credentials as Json. Should look something like:

``` json
{
  "installed": {
    "client_id": "xxxxx.apps.googleusercontent.com",
    "project_id": "xxxxx",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_secret": "xxxx",
    "redirect_uris": [
      "http://localhost"
    ]
  }
}
```

Save the project credentials as Credentials.json in the project folder

In the [[OAuth]] consent screen:
https://console.cloud.google.com/apis/credentials/consent

- Add the users that you want to use this project on as test users
- OR - actually publish the project so that everyone can use it.
	- But then your project might have to go through the google review, depending on the permissions you're trying to use.
	- So just using this as a Test Project is easier.

## First Time Running
Before starting, there's another config file: `appsettings.json` containing a field called `AccountToUse` - fill in the account you intent to use this tool on.

The purpose is that all accounts a separated in their own [[sqlite]] database. So the program needs to know where to store the results

The first time you will run this application, a new browser will open and prompt you to login with google credentials. These credentials will be saved in that sqlite database.

# Running more often
The program only loads changes since the last known modifications. So you can configure this program to run every time your computer starts, or put it in some kind of task scheduler.

You can call the program with command args `silent` and the program won't wait for console input to finish.
- The console will then also attempt to hide itself (the console window) - So it doesn't popup and show
	- This might not work on all systems or new terminals

# QA:
- Q: Hey this is kind of slow, why are you processing everything sequentially?
	- A1: Because [[Google]]/[[Google Drive]] has rate limits, so there's not much optimization to be done. 
		- If I make it faster, I'll get rate-limited.
		- And then I'll have to make it slower again, and balance the speed to respect the google rate limit
	- A2: Also [[sqlite]] is not great at multithreaded inputs
		- So I'd first have to do the calls, cache them in memory, and then later batch insert them
		- If the process stops in the middle of a batch, that data would be lost.
		- So it's just more convenient to do that sequentially as well.

- Q: Why are you using the Google Drive API from source, instead of using the package?
	- A: I copied the Google Drive API from source: https://github.com/googleapis/google-api-dotnet-client/blob/main/Src/Generated/Google.Apis.Drive.v3/Google.Apis.Drive.v3.cs
		- Because if I used the package, I'd have to create entities for all Google Drive DTOs, 
		- And I'd have to create a mapper between the Google Drive DTOs and database entities.
		- I tried just adding the file as a partial git-module, but git-moduling the repo crashes in the middle
		- Yea, it's bit hacky to dump DTOs straight into a database, but it's a lot faster to build, and less bloaty mapper

- Q: How can I view the result?
	- A: You need to install a sqlite viewer, 
		- Personally I prefer [[DataGrip]] - but that's not free
		- There are free alternatives
	- A2: [[Eventually™]] I might make an exporter to markdown 


- Q: What data is stored locally?
	- A: As much as possible! Here's an overview over everything saved: Probably best rendered in [[Obsidian]] if [[Github]] is not displaying the diagram correctly


```mermaid
classDiagram
direction BT
class Comments {
   text Anchor
   integer AuthorDatabaseId
   text Content
   text CreatedTimeRaw
   text CreatedTimeDateTimeOffset
   text CreatedTime
   integer Deleted
   text HtmlContent
   text Kind
   text ModifiedTimeRaw
   text ModifiedTimeDateTimeOffset
   text ModifiedTime
   integer QuotedFileContentDatabaseId
   integer Resolved
   text ETag
   text FileId
   text Id
}
class ContentHintsData {
   text IndexableText
   integer ThumbnailDatabaseId
   integer Id
}
class ContentRestriction {
   integer OwnerRestricted
   integer ReadOnly__
   text Reason
   integer RestrictingUserDatabaseId
   text RestrictionTimeRaw
   text RestrictionTimeDateTimeOffset
   text RestrictionTime
   integer SystemRestricted
   text Type
   text ETag
   text FileId
   integer Id
}
class File {
   text AppProperties
   integer CapabilitiesDatabaseId
   integer ContentHintsDatabaseId
   integer CopyRequiresWriterPermission
   text CreatedTimeRaw
   text CreatedTimeDateTimeOffset
   text CreatedTime
   text Description
   text DriveId
   integer ExplicitlyTrashed
   text ExportLinks
   text FileExtension
   text FolderColorRgb
   text FullFileExtension
   integer HasAugmentedPermissions
   integer HasThumbnail
   text HeadRevisionId
   text IconLink
   integer ImageMediaMetadataDatabaseId
   integer IsAppAuthorized
   text Kind
   integer LabelInfoDatabaseId
   integer LastModifyingUserDatabaseId
   integer LinkShareMetadataDatabaseId
   text Md5Checksum
   text MimeType
   integer ModifiedByMe
   text ModifiedByMeTimeRaw
   text ModifiedByMeTimeDateTimeOffset
   text ModifiedByMeTime
   text ModifiedTimeRaw
   text ModifiedTimeDateTimeOffset
   text ModifiedTime
   text Name
   text OriginalFilename
   integer OwnedByMe
   text Parents
   text PermissionIds
   text Properties
   integer QuotaBytesUsed
   text ResourceKey
   text Sha1Checksum
   text Sha256Checksum
   integer Shared
   text SharedWithMeTimeRaw
   text SharedWithMeTimeDateTimeOffset
   text SharedWithMeTime
   integer SharingUserDatabaseId
   integer ShortcutDetailsDatabaseId
   integer Size
   text Spaces
   integer Starred
   text TeamDriveId
   text ThumbnailLink
   integer ThumbnailVersion
   integer Trashed
   text TrashedTimeRaw
   text TrashedTimeDateTimeOffset
   text TrashedTime
   integer TrashingUserDatabaseId
   integer Version
   integer VideoMediaMetadataDatabaseId
   integer ViewedByMe
   text ViewedByMeTimeRaw
   text ViewedByMeTimeDateTimeOffset
   text ViewedByMeTime
   integer ViewersCanCopyContent
   text WebContentLink
   text WebViewLink
   integer WritersCanShare
   text ETag
   integer StatusType
   integer ProcessId
   text Id
}
class FileCapabilitiesData {
   integer CanAcceptOwnership
   integer CanAddChildren
   integer CanAddFolderFromAnotherDrive
   integer CanAddMyDriveParent
   integer CanChangeCopyRequiresWriterPermission
   integer CanChangeSecurityUpdateEnabled
   integer CanChangeViewersCanCopyContent
   integer CanComment
   integer CanCopy
   integer CanDelete
   integer CanDeleteChildren
   integer CanDownload
   integer CanEdit
   integer CanListChildren
   integer CanModifyContent
   integer CanModifyContentRestriction
   integer CanModifyEditorContentRestriction
   integer CanModifyLabels
   integer CanModifyOwnerContentRestriction
   integer CanMoveChildrenOutOfDrive
   integer CanMoveChildrenOutOfTeamDrive
   integer CanMoveChildrenWithinDrive
   integer CanMoveChildrenWithinTeamDrive
   integer CanMoveItemIntoTeamDrive
   integer CanMoveItemOutOfDrive
   integer CanMoveItemOutOfTeamDrive
   integer CanMoveItemWithinDrive
   integer CanMoveItemWithinTeamDrive
   integer CanMoveTeamDriveItem
   integer CanReadDrive
   integer CanReadLabels
   integer CanReadRevisions
   integer CanReadTeamDrive
   integer CanRemoveChildren
   integer CanRemoveContentRestriction
   integer CanRemoveMyDriveParent
   integer CanRename
   integer CanShare
   integer CanTrash
   integer CanTrashChildren
   integer CanUntrash
   integer Id
}
class FileOwners {
   text FilesId
   integer OwnersDatabaseId
}
class GoogleUser {
   text AccessToken
   text TokenType
   integer ExpiresInSeconds
   text RefreshToken
   text Scope
   text IdToken
   text IssuedUtc
   integer Id
}
class ImageMediaMetadataData {
   real Aperture
   text CameraMake
   text CameraModel
   text ColorSpace
   real ExposureBias
   text ExposureMode
   real ExposureTime
   integer FlashUsed
   real FocalLength
   integer Height
   integer IsoSpeed
   text Lens
   integer LocationDatabaseId
   real MaxApertureValue
   text MeteringMode
   integer Rotation
   text Sensor
   integer SubjectDistance
   text Time
   text WhiteBalance
   integer Width
   integer Id
}
class LabelFieldMapping {
   text Name
   text LabelFieldId
   text LabelId
   integer Id
}
class LabelFields {
   text DateString
   text Integer
   text Kind
   text Selection
   text Text
   text ValueType
   text ETag
   text Id
}
class LabelInfoData {
   integer Id
}
class Labels {
   text Kind
   text RevisionId
   text ETag
   integer LabelInfoDataDatabaseId
   text Id
}
class LinkShareMetadataData {
   integer SecurityUpdateEligible
   integer SecurityUpdateEnabled
   integer Id
}
class LocationData {
   real Altitude
   real Latitude
   real Longitude
   integer Id
}
class Permission {
   integer AllowFileDiscovery
   integer Deleted
   text DisplayName
   text Domain
   text EmailAddress
   text ExpirationTimeRaw
   text ExpirationTimeDateTimeOffset
   text ExpirationTime
   text Kind
   integer PendingOwner
   text PhotoLink
   text Role
   text Type
   text View
   text ETag
   text FileId
   text Id
}
class PermissionDetailsData {
   integer Inherited
   text InheritedFrom
   text PermissionType
   text Role
   text PermissionId
   integer Id
}
class Process {
   integer Status
   text Started
   text Finished
   integer Id
}
class QuotedFileContentData {
   text MimeType
   text Value
   integer Id
}
class Replies {
   text Action
   integer AuthorDatabaseId
   text Content
   text CreatedTimeRaw
   text CreatedTimeDateTimeOffset
   text CreatedTime
   integer Deleted
   text HtmlContent
   text Kind
   text ModifiedTimeRaw
   text ModifiedTimeDateTimeOffset
   text ModifiedTime
   text ETag
   text CommentId
   text FileId
   text Id
}
class ShortcutDetailsData {
   text TargetId
   text TargetMimeType
   text TargetResourceKey
   integer Id
}
class TeamDrivePermissionDetailsData {
   integer Inherited
   text InheritedFrom
   text Role
   text TeamDrivePermissionType
   text PermissionId
   integer Id
}
class ThumbnailData {
   text Image
   text MimeType
   integer Id
}
class User {
   text DisplayName
   text EmailAddress
   text Kind
   integer Me
   text PermissionId
   text PhotoLink
   text ETag
   text LabelFieldId
   integer Id
}
class VideoMediaMetadataData {
   integer DurationMillis
   integer Height
   integer Width
   integer Id
}
class sqlite_master {
   text type
   text name
   text tbl_name
   int rootpage
   text sql
}
class sqlite_sequence {
   unknown name
   unknown seq
}

Comments  -->  File : FileId.Id
Comments  -->  QuotedFileContentData : QuotedFileContentDatabaseId.Id
Comments  -->  User : AuthorDatabaseId.Id
ContentHintsData  -->  ThumbnailData : ThumbnailDatabaseId.Id
ContentRestriction  -->  File : FileId.Id
ContentRestriction  -->  User : RestrictingUserDatabaseId.Id
File  -->  ContentHintsData : ContentHintsDatabaseId.Id
File  -->  FileCapabilitiesData : CapabilitiesDatabaseId.Id
File  -->  ImageMediaMetadataData : ImageMediaMetadataDatabaseId.Id
File  -->  LabelInfoData : LabelInfoDatabaseId.Id
File  -->  LinkShareMetadataData : LinkShareMetadataDatabaseId.Id
File  -->  Process : ProcessId.Id
File  -->  ShortcutDetailsData : ShortcutDetailsDatabaseId.Id
File  -->  User : TrashingUserDatabaseId.Id
File  -->  User : LastModifyingUserDatabaseId.Id
File  -->  User : SharingUserDatabaseId.Id
File  -->  VideoMediaMetadataData : VideoMediaMetadataDatabaseId.Id
FileOwners  -->  File : FilesId.Id
FileOwners  -->  User : OwnersDatabaseId.Id
ImageMediaMetadataData  -->  LocationData : LocationDatabaseId.Id
LabelFieldMapping  -->  LabelFields : LabelFieldId.Id
LabelFieldMapping  -->  Labels : LabelId.Id
Labels  -->  LabelInfoData : LabelInfoDataDatabaseId.Id
Permission  -->  File : FileId.Id
PermissionDetailsData  -->  Permission : PermissionId.Id
Replies  -->  Comments : CommentId.Id
Replies  -->  File : FileId.Id
Replies  -->  User : AuthorDatabaseId.Id
TeamDrivePermissionDetailsData  -->  Permission : PermissionId.Id
User  -->  LabelFields : LabelFieldId.Id

```
