using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroFocus.InsecureProductService.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MicroFocus.InsecureProductService.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                // Look for any products.
                if (context.Product.Any())
                {
                    return; // DB has been seeded
                }
                else
                {
                    context.Product.AddRange(
                        new Product
                        {
                            Code = "SWA234-A568-00010",
                            Name = "Solodox 750",
                            Rating = 4,
                            Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin pharetra enim erat, sed tempor mauris viverra in. Donec ante diam, rhoncus dapibus efficitur ut, sagittis a elit. Integer non ante felis. Curabitur nec lectus ut velit bibendum euismod. Nulla mattis convallis neque ac euismod. Ut vel mattis lorem, nec tempus nibh. Vivamus tincidunt enim a risus placerat viverra. Curabitur diam sapien, posuere dignissim accumsan sed, tempus sit amet diam. Aliquam tincidunt vitae quam non rutrum. Nunc id sollicitudin neque, at posuere metus. Sed interdum ex erat, et ornare purus bibendum id. Suspendisse sagittis est dui. Donec vestibulum elit at arcu feugiat porttitor.",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin pharetra enim erat, sed tempor mauris viverra in. Donec ante diam, rhoncus dapibus efficitur ut, sagittis a elit. Integer non ante felis. Curabitur nec lectus ut velit bibendum euismod. Nulla mattis convallis neque ac euismod. Ut vel mattis lorem, nec tempus nibh. Vivamus tincidunt enim a risus placerat viverra. Curabitur diam sapien, posuere dignissim accumsan sed, tempus sit amet diam. Aliquam tincidunt vitae quam non rutrum. Nunc id sollicitudin neque, at posuere metus. Sed interdum ex erat, et ornare purus bibendum id. Suspendisse sagittis est dui. Donec vestibulum elit at arcu feugiat porttitor.",
                            Image = "generic-product-4.jpg",
                            Price = 12.95M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA534-F528-00115",
                            Name = "Alphadex Plus",
                            Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sit amet quam eget neque vestibulum tincidunt vitae vitae augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer rhoncus varius sem non luctus. Etiam tincidunt et leo non tempus. Etiam imperdiet elit arcu, a fermentum arcu commodo vel. Fusce vel consequat erat. Curabitur non lacus velit. Donec dignissim velit et sollicitudin pulvinar.",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sit amet quam eget neque vestibulum tincidunt vitae vitae augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer rhoncus varius sem non luctus. Etiam tincidunt et leo non tempus. Etiam imperdiet elit arcu, a fermentum arcu commodo vel. Fusce vel consequat erat. Curabitur non lacus velit. Donec dignissim velit et sollicitudin pulvinar.",
                            Image = "generic-product-1.jpg",
                            Price = 14.95M,
                            OnSale = true,
                            SalePrice = 9.95M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA179-G243-00101",
                            Name = "Dontax",
                            Summary = "Aenean sit amet pulvinar mauris. Suspendisse eu ligula malesuada, condimentum tortor rutrum, rutrum dui. Sed vehicula augue sit amet placerat bibendum. Maecenas ac odio libero. Donec mi neque, convallis ut nulla quis, malesuada convallis velit. Aenean a augue blandit, viverra massa nec, laoreet quam. In lacinia eros quis lacus dictum pharetra.",
                            Description = "Aenean sit amet pulvinar mauris. Suspendisse eu ligula malesuada, condimentum tortor rutrum, rutrum dui. Sed vehicula augue sit amet placerat bibendum. Maecenas ac odio libero. Donec mi neque, convallis ut nulla quis, malesuada convallis velit. Aenean a augue blandit, viverra massa nec, laoreet quam. In lacinia eros quis lacus dictum pharetra.",
                            Image = "generic-product-2.jpg",
                            Price = 8.50M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA201-D342-00132",
                            Name = "Tranix Life",
                            Summary = "Curabitur imperdiet lacus nec lacus feugiat varius. Integer hendrerit erat orci, eget varius urna varius ac. Nulla fringilla, felis eget cursus imperdiet, odio eros tincidunt est, non blandit enim ante nec magna. Suspendisse in justo maximus nisi molestie bibendum. Fusce consequat accumsan nulla, vel pharetra nulla consequat sit amet.",
                            Description = "Curabitur imperdiet lacus nec lacus feugiat varius. Integer hendrerit erat orci, eget varius urna varius ac. Nulla fringilla, felis eget cursus imperdiet, odio eros tincidunt est, non blandit enim ante nec magna. Suspendisse in justo maximus nisi molestie bibendum. Fusce consequat accumsan nulla, vel pharetra nulla consequat sit amet.",
                            Image = "generic-product-3.jpg",
                            Price = 7.95M,
                            OnSale = true,
                            SalePrice = 4.95M,
                            InStock = true,
                            TimeToStock = 14,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA312-F432-00134",
                            Name = "Salex Two",
                            Summary = "In porta viverra condimentum. Morbi nibh magna, suscipit sit amet urna sed, euismod consectetur eros. Donec egestas, elit ut commodo fringilla, sem quam suscipit lectus, id tempus enim sem quis risus. Curabitur eleifend bibendum magna, vel iaculis elit varius et. Sed mollis dolor quis metus lacinia posuere. Phasellus odio mi, tempus quis dui et, consectetur iaculis odio. Quisque fringilla viverra eleifend. Cras dignissim euismod tortor, eget congue turpis fringilla sit amet. Aenean sed semper dolor, sed ultrices felis.",
                            Description = "In porta viverra condimentum. Morbi nibh magna, suscipit sit amet urna sed, euismod consectetur eros. Donec egestas, elit ut commodo fringilla, sem quam suscipit lectus, id tempus enim sem quis risus. Curabitur eleifend bibendum magna, vel iaculis elit varius et. Sed mollis dolor quis metus lacinia posuere. Phasellus odio mi, tempus quis dui et, consectetur iaculis odio. Quisque fringilla viverra eleifend. Cras dignissim euismod tortor, eget congue turpis fringilla sit amet. Aenean sed semper dolor, sed ultrices felis.",
                            Image = "generic-product-5.jpg",
                            Price = 11.95M,
                            InStock = false,
                            TimeToStock = 14,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA654-F106-00412",
                            Name = "Betala Lite",
                            Summary = "Sed bibendum metus vitae suscipit mattis. Mauris turpis purus, sodales a egestas vel, tincidunt ac ipsum. Donec in sapien et quam varius dignissim. Phasellus eros sem, facilisis quis vehicula sed, ornare eget odio. Nam tincidunt urna mauris, id tincidunt risus posuere ac. Integer vel est vel enim convallis blandit sed sed urna. Nam dapibus erat nunc, id euismod diam pulvinar id. Fusce a felis justo.",
                            Description = "Sed bibendum metus vitae suscipit mattis. Mauris turpis purus, sodales a egestas vel, tincidunt ac ipsum. Donec in sapien et quam varius dignissim. Phasellus eros sem, facilisis quis vehicula sed, ornare eget odio. Nam tincidunt urna mauris, id tincidunt risus posuere ac. Integer vel est vel enim convallis blandit sed sed urna. Nam dapibus erat nunc, id euismod diam pulvinar id. Fusce a felis justo.",
                            Image = "generic-product-4.jpg",
                            Price = 11.95M,
                            OnSale = true,
                            SalePrice = 9.95M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA254-A971-00213",
                            Name = "Stimlab Mitre",
                            Summary = "Phasellus malesuada pulvinar justo, ac eleifend magna lacinia eget. Proin vulputate nec odio at volutpat. Duis non suscipit arcu. Nam et arcu vehicula, sollicitudin eros non, scelerisque diam. Phasellus sagittis pretium tristique. Vestibulum sit amet lectus nisl. Aliquam aliquet dolor sit amet neque placerat, vel varius metus molestie. Fusce sed ipsum blandit, efficitur est vitae, scelerisque enim. Integer porttitor est et dictum blandit. Quisque gravida tempus orci nec finibus.",
                            Description = "Phasellus malesuada pulvinar justo, ac eleifend magna lacinia eget. Proin vulputate nec odio at volutpat. Duis non suscipit arcu. Nam et arcu vehicula, sollicitudin eros non, scelerisque diam. Phasellus sagittis pretium tristique. Vestibulum sit amet lectus nisl. Aliquam aliquet dolor sit amet neque placerat, vel varius metus molestie. Fusce sed ipsum blandit, efficitur est vitae, scelerisque enim. Integer porttitor est et dictum blandit. Quisque gravida tempus orci nec finibus.",
                            Image = "generic-product-6.jpg",
                            Price = 12.95M,
                            InStock = false,
                            TimeToStock = 7,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA754-B418-00315",
                            Name = "Alphadex Lite",
                            Summary = "Nam bibendum porta metus. Aliquam viverra pulvinar velit et condimentum. Pellentesque quis purus libero. Fusce hendrerit tortor sed nulla lobortis commodo. Donec ultrices mi et sollicitudin aliquam. Phasellus rhoncus commodo odio quis faucibus. Nullam interdum mi non egestas pellentesque. Duis nec porta leo, eu placerat tellus.",
                            Description = "Nam bibendum porta metus. Aliquam viverra pulvinar velit et condimentum. Pellentesque quis purus libero. Fusce hendrerit tortor sed nulla lobortis commodo. Donec ultrices mi et sollicitudin aliquam. Phasellus rhoncus commodo odio quis faucibus. Nullam interdum mi non egestas pellentesque. Duis nec porta leo, eu placerat tellus.",
                            Image = "generic-product-7.jpg",
                            Price = 9.95M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA432-E901-00126",
                            Name = "Villacore 2000",
                            Summary = "Aliquam erat volutpat. Ut gravida scelerisque purus a sagittis. Nullam pellentesque arcu sed risus dignissim scelerisque. Maecenas vel elit pretium, ultrices augue ac, interdum libero. Suspendisse potenti. In felis metus, mattis quis lorem congue, condimentum volutpat felis. Nullam mauris mi, bibendum in ultrices sed, blandit congue ipsum.",
                            Description = "Aliquam erat volutpat. Ut gravida scelerisque purus a sagittis. Nullam pellentesque arcu sed risus dignissim scelerisque. Maecenas vel elit pretium, ultrices augue ac, interdum libero. Suspendisse potenti. In felis metus, mattis quis lorem congue, condimentum volutpat felis. Nullam mauris mi, bibendum in ultrices sed, blandit congue ipsum.",
                            Image = "generic-product-8.jpg",
                            Price = 19.95M,
                            InStock = true,
                            TimeToStock = 30,
                            Available = true
                        },

                        new Product
                        {
                            Code = "SWA723-A375-00412",
                            Name = "Kanlab Blue",
                            Summary = "Proin eget nisl non sapien gravida pellentesque. Cras tincidunt tortor posuere, laoreet sapien nec, tincidunt nunc. Integer vehicula, erat ut pretium porta, velit leo dignissim odio, eu ultricies urna nulla a dui. Proin et dapibus turpis, et tincidunt augue. In mattis luctus elit, in vehicula erat pretium sed. Suspendisse ullamcorper mollis dolor eu tristique.",
                            Description = "Proin eget nisl non sapien gravida pellentesque. Cras tincidunt tortor posuere, laoreet sapien nec, tincidunt nunc. Integer vehicula, erat ut pretium porta, velit leo dignissim odio, eu ultricies urna nulla a dui. Proin et dapibus turpis, et tincidunt augue. In mattis luctus elit, in vehicula erat pretium sed. Suspendisse ullamcorper mollis dolor eu tristique.",
                            Image = "generic-product-9.jpg",
                            Price = 9.95M,
                            InStock = false,
                            TimeToStock = 7,
                            Available = true
                        }

                    );
                }
                
                context.SaveChanges();
            }
        }

    }
}
