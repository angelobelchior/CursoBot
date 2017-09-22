using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using AdaptiveCards;
using Newtonsoft.Json.Linq;

namespace CursoBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if(activity.Value != null)
            {
                var response = activity.Value as JObject;
                var name = response["name"].Value<string>();
                var email = response["email"].Value<string>();
                var phone = response["phone"].Value<string>();

                await context.PostAsync($"Nome: {name}. E-mail: {email}. Telefone: {phone}");
                context.Wait(MessageReceivedAsync);
                return;
            }

            var message = activity.CreateReply();
            if (activity.Text.Equals("herocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var herocard = GetHeroCard();
                message.Attachments.Add(herocard);
            }
            else if (activity.Text.Equals("thumb", StringComparison.InvariantCultureIgnoreCase))
            {
                var thumb = GetThumbnailCard();
                message.Attachments.Add(thumb);
            }
            else if (activity.Text.Equals("gif", StringComparison.InvariantCultureIgnoreCase))
            {
                var git = GetAnimationCard();
                message.Attachments.Add(git);
            }
            else if (activity.Text.Equals("video", StringComparison.InvariantCultureIgnoreCase))
            {
                var video = GetVideoCard();
                message.Attachments.Add(video);
            }
            else if (activity.Text.Equals("audio", StringComparison.InvariantCultureIgnoreCase))
            {
                var audio = GetAudioCard();
                message.Attachments.Add(audio);
            }
            else if (activity.Text.Equals("carousel", StringComparison.InvariantCultureIgnoreCase))
            {
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                var herocard = GetHeroCard();
                message.Attachments.Add(herocard);

                var thumb = GetThumbnailCard();
                message.Attachments.Add(thumb);

                var git = GetAnimationCard();
                message.Attachments.Add(git);
            }
            else if (activity.Text.Equals("receipt", StringComparison.InvariantCultureIgnoreCase))
            {
                var receipt = GetReceiptCard();
                message.Attachments.Add(receipt);
            }
            else if (activity.Text.Equals("login", StringComparison.InvariantCultureIgnoreCase))
            {
                var login = GetSigninCard();
                message.Attachments.Add(login);
            }
            else if (activity.Text.Equals("adaptive", StringComparison.InvariantCultureIgnoreCase))
            {
                var adaptive = GetAdaptiveCard();
                message.Attachments.Add(adaptive);
            }
            else if (activity.Text.Equals("weather", StringComparison.InvariantCultureIgnoreCase))
            {
                var weather = GetWeather();
                message.Attachments.Add(weather);
            }
            else if (activity.Text.Equals("form", StringComparison.InvariantCultureIgnoreCase))
            {
                var form = GetFormAdaptiveCard();
                message.Attachments.Add(form);
            }

            await context.PostAsync(message);
            await context.PostAsync("Esse é um texto em **negrito**");

            context.Wait(MessageReceivedAsync);
        }

        private Attachment GetHeroCard()
        {
            var heroCard = new HeroCard()
            {
                Title = "Rivelino",
                Subtitle = "Roberto Rivelino",
                Images = new List<CardImage>
                {
                    new CardImage("https://www.imortaisdofutebol.com/wp-content/uploads/2012/09/roberto-rivelino2.jpg", "Roberto Rivelino")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Wikipedia", null, "https://pt.wikipedia.org/wiki/Roberto_Rivellino")
                }
            };

            return heroCard.ToAttachment();
        }

        private Attachment GetThumbnailCard()
        {
            var thumb = new ThumbnailCard()
            {
                Title = "Pelé",
                Subtitle = "Rei Pelé",
                Images = new List<CardImage>
                {
                    new CardImage("https://www.imortaisdofutebol.com/wp-content/uploads/2012/12/pele-1.jpg", "Rei Pelé")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Wikipedia", null, "https://pt.wikipedia.org/wiki/Pel%C3%A9")
                }
            };

            return thumb.ToAttachment();
        }

        private Attachment GetAnimationCard()
        {
            var animation = new AnimationCard
            {
                Title = "Pelé",
                Subtitle = "Entortou o goleiro mas perdeu o gol :/",
                Image = new ThumbnailUrl("https://www.imortaisdofutebol.com/wp-content/uploads/2012/12/pele-1.jpg"),
                Autostart = true,
                Autoloop = true,
                Media = new List<MediaUrl>
                {
                    new MediaUrl("https://i.makeagif.com/media/5-16-2016/c100k7.gif")
                }
            };
            return animation.ToAttachment();
        }

        private Attachment GetVideoCard()
        {
            var video = new VideoCard
            {
                Title = "Um filme",
                Subtitle = "Mais um filme fofo",
                Image = new ThumbnailUrl("https://www.imortaisdofutebol.com/wp-content/uploads/2012/12/pele-1.jpg"),
                Autostart = true,
                Autoloop = true,
                Media = new List<MediaUrl>
                {
                    new MediaUrl("http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Title = "Ver em full screen",
                        Value = "http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4"
                    }
                }
            };
            return video.ToAttachment();
        }

        private Attachment GetAudioCard()
        {
            var audio = new AudioCard
            {
                Title = "Esse é áudio",
                Subtitle = "Mais um adio sinistro",
                Image = new ThumbnailUrl("https://www.imortaisdofutebol.com/wp-content/uploads/2012/09/roberto-rivelino2.jpg"),
                Autostart = true,
                Autoloop = true,
                Media = new List<MediaUrl>
                {
                    new MediaUrl("http://www.wavlist.com/movies/004/father.wav")
                }
            };
            return audio.ToAttachment();
        }

        private Attachment GetReceiptCard()
        {
            var receipt = new ReceiptCard
            {
                Title = "Itens de Compra",
                Facts = new List<Microsoft.Bot.Connector.Fact>
                {
                    new Microsoft.Bot.Connector.Fact("Número da Compra", "1234"),
                    new Microsoft.Bot.Connector.Fact("Endereço de Entrega", "Rua das Alamedas"),
                    new Microsoft.Bot.Connector.Fact("Meio de Pagamento", "Cartão XPTO final 1234")
                },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem("xbox one",
                                    "Console",
                                    "Aqui ven um texto",
                                    new CardImage("http://images.eurogamer.net/2017/articles/1/9/0/6/2/1/8/xbox-one-x-price-release-date-specs-games-vr-project-scorpio-149722316665.jpg/EG11/resize/600x-1/quality/80/format/jpg"),
                                    "R$:1800.00",
                                    "1 console"),

                    new ReceiptItem("controle xbox one",
                                    image: new CardImage("http://www.casasbahia-imagens.com.br/Control/ArquivoExibir.aspx?IdArquivo=818216795"),
                                    price: "R$:200.00",
                                    quantity: "1"),

                    new ReceiptItem("Minecraft",
                                    image: new CardImage("https://images-americanas.b2w.io/produtos/01/00/item/120812/5/120812572SZ.jpg"),
                                    price: "R$:120.00",
                                    quantity: "1"),
                },
                Tax = "R$800.00",
                Total = "R$2120.00",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Finalizar compra")
                }
            };

            return receipt.ToAttachment();
        }

        private Attachment GetSigninCard()
        {
            var signin = new SigninCard
            {
                Text = "Login",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.Signin, "Sign-in", value:"https://login.microsoftonline.com/")
                }
            };

            return signin.ToAttachment();
        }

        public Attachment GetAdaptiveCard()
        {
            var card = new AdaptiveCard
            {
                Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>"
            };

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = "Adaptive Card design session",
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = "Conf Room 112/3377 (10)"
            });

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = "12:30 PM - 1:30 PM"
            });

            // Add list of choices to the card.
            card.Body.Add(new ChoiceSet()
            {
                Id = "snooze",
                Style = ChoiceInputStyle.Expanded,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "5 minutes", Value = "5", IsSelected = true },
                    new Choice() { Title = "15 minutes", Value = "15" },
                    new Choice() { Title = "30 minutes", Value = "30" }
                }
            });

            // Add buttons to the card.
            card.Actions.Add(new HttpAction()
            {
                Title = "Snooze"
            });

            card.Actions.Add(new HttpAction()
            {
                Title = "I'll be late"
            });

            card.Actions.Add(new HttpAction()
            {
                Title = "Dismiss"
            });

            var attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }

        /**/
        private Attachment GetWeather()
        {
            var card = new AdaptiveCard();
            card.Speak = $"<s>Hoje está um dia agradável</s>";
            AddCurrentWeather(card);
            AddForecast(card);
            var attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }
        private void AddCurrentWeather(AdaptiveCard card)
        {
            var current = new ColumnSet();
            card.Body.Add(current);
            var currentColumn = new Column();
            current.Columns.Add(currentColumn);
            currentColumn.Size = "35";
            var currentImage = new Image();
            currentColumn.Items.Add(currentImage);
            currentImage.Url = "https://cdn.apixu.com/weather/64x64/night/113.png";
            var currentColumn2 = new Column();
            current.Columns.Add(currentColumn2);
            currentColumn2.Size = "65";
            string date = DateTime.Now.DayOfWeek.ToString();
            AddTextBlock(currentColumn2, $"São Paulo ({date})", TextSize.Large, false);
            AddTextBlock(currentColumn2, $"22° C", TextSize.Large);
            AddTextBlock(currentColumn2, $"Tempo Lindo", TextSize.Medium);
        }
        private void AddForecast(AdaptiveCard card)
        {
            var forecast = new ColumnSet();
            card.Body.Add(forecast);
            for (int i = 0; i < 4; i++)
            {
                var date = DateTime.Now.AddDays(i + 1);
                var column = new Column();
                AddForcastColumn(forecast, column);
                AddTextBlock(column, date.DayOfWeek.ToString().Substring(0, 3), TextSize.Medium);
                AddImageColumn(column);
                AddTextBlock(column, $"22/23", TextSize.Medium);
            }
        }
        private void AddImageColumn(Column column)
        {
            var image = new Image();
            image.Size = ImageSize.Auto;
            image.Url = "https://cdn.apixu.com/weather/64x64/day/116.png";
            column.Items.Add(image);
        }
        private string GetIconUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            if (url.StartsWith("http"))
                return url;
            //some clients do not accept \\
            return "https:" + url;
        }
        private void AddForcastColumn(ColumnSet forecast, Column column)
        {
            forecast.Columns.Add(column);
            column.Size = "20";
            var action = new OpenUrlAction();
            action.Url = "https://www.bing.com/search?q=forecast in São Paulo";
            column.SelectAction = action;
        }
        private void AddTextBlock(Column column, string text, TextSize size, bool isSubTitle = true)
        {
            column.Items.Add(new TextBlock()
            {
                Text = text,
                Size = size,
                HorizontalAlignment = HorizontalAlignment.Center,
                IsSubtle = isSubTitle,
                Separation = SeparationStyle.None
            });
        }

        public Attachment GetFormAdaptiveCard()
        {
            var card = new AdaptiveCard
            {
                Speak = "<s>Formulário</s>"
            };
            var columnSet = new ColumnSet();
            columnSet.Columns.Add(new Column
            {
                Size = "2",
                Type = "Column",
                Items = new List<CardElement>
                {
                    new TextBlock
                    {
                        Text = "Preencha as informações",
                        Weight=  TextWeight.Bolder,
                        Size=  TextSize.Large
                    },
                    new TextBlock
                    {
                        Text = "Para prosseguirmos, precisamos que informe os dados abaixo.",
                        IsSubtle = true,
                        Wrap = true
                    },

                    new TextBlock
                    {
                        Text = "Não se preocupe. Suas informações estarão seguras conosco",
                        IsSubtle = true,
                        Wrap = true,
                        Size=  TextSize.Small
                    },

                    new TextBlock
                    {
                        Text = "Seu nome",
                        Wrap = true
                    },
                    new TextInput
                    {
                        Id="name",
                        Placeholder="Nome Completo",
                        IsRequired = true
                    },

                    new TextBlock
                    {
                        Text = "Seu e-mail",
                        Wrap = true
                    },
                    new TextInput
                    {
                        Id="email",
                        Placeholder="seu@email.com",
                        IsRequired = true,
                        Style= TextInputStyle.Email
                    },

                    new TextBlock
                    {
                        Text = "Telefone"
                    },
                    new TextInput
                    {
                        Id="phone",
                        Placeholder="(12)3456-7890",
                        IsRequired = true,
                        Style= TextInputStyle.Tel
                    },
                }
            });
            columnSet.Columns.Add(new Column
            {
                Size = "1",
                Items = new List<CardElement>
                {
                    new Image
                    {
                        Url = "https://image.freepik.com/icones-gratis/icone-de-cadeado_318-42569.jpg",
                        Size = ImageSize.Auto
                    }
                }
            });
            card.Body.Add(columnSet);
            card.Actions.Add(new SubmitAction()
            {
                Title = "Submit"
            });
            var attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }
        /**/
    }
}