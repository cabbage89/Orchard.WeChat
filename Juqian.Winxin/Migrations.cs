using Orchard.Data.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Title.Models;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement;
using Orchard.Settings;
using Orchard;

namespace Juqian.Winxin
{
    public class Migrations : DataMigrationImpl
    {
        private readonly ISiteService _siteService;
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        public Migrations(IContentManager contentManager
            , ISiteService siteService, IOrchardServices orchardServices)
        {
            _contentManager = contentManager;
            _siteService = siteService;
            _orchardServices = orchardServices;
        }

        public int Create()
        {
            #region 初始化微信内容类型
            {
                ContentDefinitionManager.AlterPartDefinition("WXTextMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXTextMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("WXTextMsg")
                    .WithPart("TitlePart")
                    .DisplayedAs("微信文本消息")
                    .WithSetting("Description", "回复用户文本消息.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXTextMsg", partBuilder => partBuilder
                    .WithField("WXMsgTextField", fieldBuilder => fieldBuilder.OfType("TextField")
                        .WithDisplayName("文本")
                        .WithSetting("TextFieldSettings.Required", "true")
                        .WithSetting("TextFieldSettings.Flavor", "Textarea")));
            }
            {
                ContentDefinitionManager.AlterPartDefinition("WXImageMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXImageMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("WXImageMsg")
                    .WithPart("TitlePart")
                    .DisplayedAs("微信图片消息")
                    .WithSetting("Description", "回复用户图片消息.")
                    .Creatable()
                    );
                ContentDefinitionManager.AlterPartDefinition("WXImageMsg", partBuilder => partBuilder
                    .WithField("WXMsgImageField", fieldBuilder => fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName("图片")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "true")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "false")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "1M，支持JPG格式"))
                    .WithField("WXMsgImageMediaIdField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("MediaId")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成MediaId."))
                    .WithField("WXMsgImageMediaCreateAtField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("创建时间(整形)")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成."))
                        );
            }
            {
                ContentDefinitionManager.AlterPartDefinition("WXVoiceMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXVoiceMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .WithPart("WXVoiceMsg")
                    .DisplayedAs("微信语音消息")
                    .WithSetting("Description", "回复用户语音消息.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXVoiceMsg", partBuilder => partBuilder
                    .WithField("WXMsgVoiceField", fieldBuilder => fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName("语音")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "true")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "false")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Audio")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "2M，播放长度不超过60s，支持AMR\\MP3格式"))
                    .WithField("WXMsgVoiceMediaIdField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("MediaId")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成MediaId."))
                    .WithField("WXMsgVoiceMediaCreateAtField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("创建时间(整形)")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成.")));
            }
            {
                ContentDefinitionManager.AlterPartDefinition("WXVideoMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXVideoMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("WXVideoMsg")
                    .WithPart("TitlePart")
                    .DisplayedAs("微信视频消息")
                    .WithSetting("Description", "回复用户视频消息.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXVideoMsg", partBuilder => partBuilder
                    .WithField("WXMsgVideoField", fieldBuilder => fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName("视频")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "true")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "false")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Video")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "10MB，支持MP4格式"))
                    .WithField("WXMsgVideoTitleField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("视频标题")
                        .WithSetting("InputFieldSettings.Required", "true"))
                    .WithField("WXMsgVideoDescField", fieldBuilder => fieldBuilder.OfType("TextField")
                        .WithDisplayName("视频描述")
                        .WithSetting("TextFieldSettings.Required", "true")
                        .WithSetting("TextFieldSettings.Flavor", "Textarea"))
                    .WithField("WXMsgVideoMediaIdField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("MediaId")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成MediaId."))
                    .WithField("WXMsgVideoMediaCreateAtField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("创建时间(整形)")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成."))
                    );
            }
            {
                ContentDefinitionManager.AlterPartDefinition("WXMusicMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXMusicMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("WXMusicMsg")
                    .WithPart("TitlePart")
                    .DisplayedAs("微信音乐消息")
                    .WithSetting("Description", "回复用户音乐消息.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXMusicMsg", partBuilder => partBuilder
                    .WithField("WXMsgMusicTitleField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("音乐标题")
                        .WithSetting("InputFieldSettings.Required", "true"))

                    .WithField("WXMsgMusicDescField", fieldBuilder => fieldBuilder.OfType("TextField")
                        .WithDisplayName("音乐描述")
                        .WithSetting("TextFieldSettings.Required", "true")
                        .WithSetting("TextFieldSettings.Flavor", "Textarea"))

                    .WithField("WXMsgMusicUrlField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("音乐链接")
                        .WithSetting("InputFieldSettings.Required", "true"))

                    .WithField("WXMsgHQMusicUrlField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("音乐高质量链接")
                        .WithSetting("InputFieldSettings.Required", "true"))

                    .WithField("WXMsgMusicThumbField", fieldBuilder => fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName("音乐缩略图")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "true")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Audio")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "64KB，支持JPG格式"))

                    .WithField("WXMsgMusicThumbMediaIdField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("MediaId")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成MediaId."))
                    .WithField("WXMsgMusicThumbMediaCreateAtField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("创建时间(整形)")
                        .WithSetting("InputFieldSettings.Hint", "此值可以不手动填写,程序会自动上传媒体文件并生成."))
                    );
            }
            {
                ContentDefinitionManager.AlterPartDefinition("WXNewsItemMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXNewsItemMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .WithPart("WXNewsItemMsg")
                    .DisplayedAs("微信图文消息项")
                    .WithSetting("Description", "包含在图文消息中的项目.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXNewsItemMsg", partBuilder => partBuilder
                    .WithField("WXMsgNewsItemTitleField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("标题")
                        .WithSetting("InputFieldSettings.Required", "true"))
                    .WithField("WXMsgNewsItemDescField", fieldBuilder => fieldBuilder.OfType("TextField")
                        .WithDisplayName("描述")
                        .WithSetting("TextFieldSettings.Required", "true")
                        .WithSetting("TextFieldSettings.Flavor", "Textarea"))
                    .WithField("WXMsgNewsItemUrlField", fieldBuilder => fieldBuilder.OfType("InputField")
                        .WithDisplayName("原文链接")
                        .WithSetting("InputFieldSettings.Required", "true"))
                    .WithField("WXMsgNewsItemPicField", fieldBuilder => fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName("图片")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "true")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image"))
                    );

                ContentDefinitionManager.AlterPartDefinition("WXNewsMsg", builder => builder.Attachable());
                ContentDefinitionManager.AlterTypeDefinition("WXNewsMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .WithPart("WXNewsMsg")
                    .DisplayedAs("微信图文消息")
                    .WithSetting("Description", "回复用户图文消息,可包含不超过10条子图片消息.")
                    .Creatable()
                    );

                ContentDefinitionManager.AlterPartDefinition("WXNewsMsg", partBuilder => partBuilder
                    .WithField("WXMsgNewsContainerField", fieldBuilder => fieldBuilder.OfType("ContentPickerField")
                        .WithDisplayName("包含的消息子项目")
                        .WithSetting("ContentPickerFieldSettings.Required", "true")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "true")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "WXNewsItemMsg")
                        .WithSetting("ContentPickerFieldSettings.Hint", "一次选择的消息项不得超过十个.")));
            }
            {
                ContentDefinitionManager.AlterTypeDefinition("WXDKFMsg", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .DisplayedAs("微信转发到多客服")
                    .WithSetting("Description", "把这次发送的消息转到多客服系统。用户被客服接入以后，客服关闭会话以前，处于会话过程中，用户发送的消息均会被直接转发至客服系统。")
                    );

                var page = _contentManager.Create("WXDKFMsg");
                page.As<TitlePart>().Title = "转发到多客服";
                page.As<ICommonPart>().Owner = _orchardServices.WorkContext.CurrentUser;
            }
            #endregion

            //事件菜单项
            ContentDefinitionManager.AlterTypeDefinition("WXClickMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("WeiXinClickMenuItemPart")
                .DisplayedAs("微信事件菜单项")
                .WithSetting("Description", "用户点击此按钮后，微信服务器推送事件消息(带Key值).")
                .WithSetting("Stereotype", "MenuItem")
                );

            //微信消息数据库记录对象
            SchemaBuilder.CreateTable("WXMsgPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("ToUserName")
                    .Column<string>("FromUserName")
                    .Column<string>("CreateTime")
                    .Column<string>("MsgType")
                    .Column<string>("Content")
                    .Column<double>("Location_X")
                    .Column<double>("Location_Y")
                    .Column<int>("Scale")
                    .Column<string>("Label")
                    .Column<string>("PicUrl")
                    .Column<string>("EventKey")
                    .Column<string>("Event")
                    .Column<int>("MsgId")
                    .Column<string>("MediaId")
                    .Column<string>("Format")
                    .Column<string>("ThumbMediaId")
                    .Column<string>("Title")
                    .Column<string>("Description")
                    .Column<string>("Url")
                    .Column<string>("Ticket")
                    .Column<double>("Latitude")
                    .Column<double>("Longitude")
                    .Column<double>("Precision")
                    .Column<string>("Recongnition")
                    .Column<string>("XML", column => column.WithLength(4001))
                );

            //微信消息Part
            ContentDefinitionManager.AlterPartDefinition("WXMsgPart", builder => builder.Attachable());
            ContentDefinitionManager.AlterTypeDefinition("WeiXinMsg",
                cfg => cfg
                    .WithPart("WXMsgPart")
                    .WithPart("CommonPart")
                );

            return 1;
        }

        //public int UpdateFrom1()
        //{
        //    return 2;
        //}
    }
}