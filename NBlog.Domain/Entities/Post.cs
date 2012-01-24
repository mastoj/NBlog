using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TJ.Extensions;

namespace NBlog.Domain.Entities
{
    [Serializable]
    public class PostMetaData
    {
        public string ShortUrl { get; set; }
        public string Title { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> Categories { get; set; }
        public string Excerpt { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PostMetaData)) return false;
            return Equals((PostMetaData) obj);
        }
        public bool Equals(PostMetaData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equal = Tags.AreListsEqual(other.Tags);
            equal = equal && Categories.AreListsEqual(other.Categories);
            equal = equal && other.ShortUrl == ShortUrl &&
                    Title == other.Title &&
                    Excerpt == other.Excerpt;

            return equal;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (ShortUrl != null ? ShortUrl.GetHashCode() : 0);
                result = (result * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                result = (result * 397) ^ (Tags != null ? Tags.GetHashCode() : 0);
                result = (result*397) ^ (Categories != null ? Categories.GetHashCode() : 0);
                result = (result*397) ^ (Excerpt != null ? Excerpt.GetHashCode() : 0);
                return result;
            }
        }
    }

    [Serializable]
    public class PostContent
    {
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PostContent)) return false;
            return Equals((PostContent) obj);
        }

        public bool Equals(PostContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equal = Content == other.Content &&
                        IsPublished == other.IsPublished &&
                        PublishDate == other.PublishDate &&
                        LastUpdateDate == other.LastUpdateDate;
            return equal;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Content != null ? Content.GetHashCode() : 0);
                result = (result*397) ^ IsPublished.GetHashCode();
                result = (result*397) ^ PublishDate.GetHashCode();
                result = (result * 397) ^ LastUpdateDate.GetHashCode();
                return result;
            }
        }
    }

    public class Post : Entity
    {
        public IList<PostContent> PostVersions { get; set; }
        public PostMetaData PostMetaData { get; set; }
        public PostContent PublishedPost { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Post)) return false;
            return Equals((Post) obj);
        }

        public bool Equals(Post other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equal = PostVersions.AreListsEqual(other.PostVersions);
            equal = equal && PostMetaData == other.PostMetaData;
            equal = equal && PublishedPost == PublishedPost;
            return equal;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (PostVersions != null ? PostVersions.GetHashCode() : 0);
                result = (result*397) ^ (PostMetaData != null ? PostMetaData.GetHashCode() : 0);
                result = (result*397) ^ (PublishedPost != null ? PublishedPost.GetHashCode() : 0);
                return result;
            }
        }
    }

    public static class ListHelper
    {
        public static bool AreListsEqual<T>(this IList<T> list, IList<T> listToCompare)
        {
            if (list.IsNull())
            {
                if (listToCompare.IsNull())
                {
                    return true;
                }
                return false;
            }
            if (listToCompare.IsNull())
            {
                return false;
            }
            var expectedCount = list.Count;
            var areEqual = expectedCount == listToCompare.Count;
            areEqual = areEqual && list.Where(y => listToCompare.Contains(y)).Count() == expectedCount;
            return areEqual;
        }
    }
}
