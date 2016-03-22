﻿using System;
using ExpressMapper.Tests.Model.Models;
using ExpressMapper.Tests.Model.ViewModels;
using NUnit.Framework;

namespace ExpressMapper.Tests
{
    [TestFixture]
    public class FlattenTests : BaseTestClass
    {
        [Test]
        public void FlattenFatherSonGrandsonDtoOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto();
            Mapper.Map(Father.CreateOne(), dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);    //This is mapped by the normal AutoMapper 
            Assert.AreEqual(1, dto.MyInt);              //This is mapped by the normal AutoMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
            Assert.AreEqual(3, dto.SonGrandsonMyInt);
        }


        [Test]
        public void FlattenFatherSonGrandsonDtoNoSon()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto();
            var src = new Father
            {
                MyString = "Father",
                MyInt = 1
            };
            Mapper.Map(src, dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);    //This is mapped by the normal AutoMapper 
            Assert.AreEqual(1, dto.MyInt);              //This is mapped by the normal AutoMapper 
            Assert.AreEqual(null, dto.SonMyString);
            Assert.AreEqual(0, dto.SonMyInt);
            Assert.AreEqual(null, dto.SonGrandsonMyString);
            Assert.AreEqual(0, dto.SonGrandsonMyInt);
        }

        [Test]
        public void FlattenFatherSonGrandsonDtoOverrideSonGrandsonMyStringOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>()
                .Member(dest => dest.SonGrandsonMyString, src => src.MyString).FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto();
            Mapper.Map(Father.CreateOne(), dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual("Father", dto.SonGrandsonMyString);
        }

        [Test]
        public void FlattenFatherSonGrandsonDtoIgnoreSonMyStringOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>()
                .Ignore(dest => dest.SonMyString).FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto();
            Mapper.Map(Father.CreateOne(), dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual(null, dto.SonMyString);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
        }

        [Test]
        public void FlattenFatherSonsCountDtoOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<FatherSons, FlattenFatherSonsCountDto>().FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonsCountDto();
            Mapper.Map(FatherSons.CreateOne(), dto);

            //VERIFY  
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual(5, dto.SonsCount);
        }

        [Test]
        public void FlattenLinqCollectionMethodsDtoOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<FatherSons, FlattenLinqCollectionMethodsDto>().FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenLinqCollectionMethodsDto();
            Mapper.Map(FatherSons.CreateOne(), dto);

            //VERIFY  
            Assert.AreEqual(true, dto.SonsAny);
            Assert.AreEqual(5, dto.SonsCount);
            Assert.AreEqual(5, dto.SonsLongCount);
            Assert.AreEqual("Son", dto.SonsFirstOrDefault.MyString);
        }

        [Test]
        public void FlattenCircularReferenceDtoOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<FlattenCircularReference, FlattenCircularReferenceDto>().FlattenSource();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenCircularReferenceDto();
            Mapper.Map(FlattenCircularReference.CreateOne(), dto);

            //VERIFY  
            Assert.AreEqual("Outer", dto.MyString);
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual("Inner", dto.CircularRefMyString);
        }

        //----------------------------------
        //Failure tests

        [Test]
        public void FlattenFatherSonsCountBadDtoOk()
        {
            //SETUP

            //ATTEMPT
            var ex = Assert.Throws<InvalidOperationException>( () => Mapper.Register<FatherSons, FlattenFatherSonsCountBadDto>().FlattenSource());

            //VERIFY  
            Assert.AreEqual("We could not find the Method Count() which matched the property SonsCount of type System.String.", ex.Message);
        }
    }
}