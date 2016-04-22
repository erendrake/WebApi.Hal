using System;
using System.Collections.Generic;
using System.Linq;
using iUS.WebApi.Hal.Exceptions;
using iUS.WebApi.Hal.Interfaces;

namespace iUS.WebApi.Hal
{
    public class Hypermedia : IHypermediaResolver, IHypermediaBuilder
    {
        private readonly IDictionary<Type, Link> selfLinks = new Dictionary<Type, Link>();
        private readonly IDictionary<Type, IList<Link>> hypermedia = new Dictionary<Type, IList<Link>>();
        private readonly IDictionary<Type, object> appenders = new Dictionary<Type, object>();

        private Hypermedia()
        {
        }

        public static IHypermediaBuilder CreateBuilder()
        {
            return new Hypermedia();
        }

        public void RegisterAppender<T>(IHypermediaAppender<T> appender) where T : class, IResource
        {
            if (appender == null)
                throw new ArgumentNullException(nameof(appender));

            Type type = typeof(T);

            if (appenders.ContainsKey(type))
                throw new DuplicateHypermediaResolverRegistrationException(type);

            appenders.Add(type, appender);
        }

        public void RegisterSelf<T>(Link link) where T : IResource
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            Type type = typeof(T);

            if (selfLinks.ContainsKey(type))
                throw new DuplicateSelfLinkRegistrationException(type);

            selfLinks.Add(type, link);
        }

        public void RegisterSelf<T>(Link<T> link) where T : class, IResource
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            Type type = typeof(T);

            if (selfLinks.ContainsKey(type))
                throw new DuplicateSelfLinkRegistrationException(type);

            selfLinks.Add(type, link);
        }

        public void RegisterLinks<T>(params Link[] links) where T : class, IResource
        {
            if (links == null)
                throw new ArgumentNullException(nameof(links));

            Type type = typeof(T);

            if (hypermedia.ContainsKey(type))
                hypermedia[type] = hypermedia[type].Union(links).Distinct(Link.EqualityComparer).ToList();
            else
                hypermedia.Add(type, links.Distinct(Link.EqualityComparer).ToList());
        }

        public IHypermediaResolver Build()
        {
            return this;
        }

        public IHypermediaAppender<T> ResolveAppender<T>(T resource) where T: class, IResource
        {
            Type type = resource.GetType();

            if (!appenders.ContainsKey(type)) 
                return null;
            
            return (IHypermediaAppender<T>) appenders[type];
        }

        public IEnumerable<Link> ResolveLinks(IResource resource)
        {
            Type type = resource.GetType();
            
            return hypermedia.ContainsKey(type)
                ? hypermedia[type]
                : new Link[0];
        }

        public string ResolveRel(IResource resource)
        {
            Type type = resource.GetType();

            return selfLinks.ContainsKey(type)
                ? selfLinks[type].Rel
                : type.Name.ToLowerInvariant();
        }

        public Link ResolveSelf(IResource resource)
        {
            Type type = resource.GetType();

            if (!selfLinks.ContainsKey(type))
                return null;

            Link clone = selfLinks[type].Clone();

            clone.Rel = Link.RelForSelf;

            return clone;
        }
    }
}