// implement carousel component

import { useEffect } from "react";

const Carousel = ({ children, currentPage, vertical }) => {
  const carousel = document.querySelector(".carousel");
  const carouselItems = document.querySelectorAll(".carousel-item");

  const handlePageChange = () => {
    if (carousel && carouselItems) {
      if (vertical) {
        carousel.style.transform = `translateY(-${currentPage * 100}%)`;
      } else {
        carousel.style.transform = `translateX(-${currentPage * 100}%)`;
      }
    }
  };

  useEffect(() => {
    handlePageChange();
  }, [currentPage, vertical, children]);

  useEffect(() => {
    handlePageChange();
  }, []);

  return (
    <div className="carousel">
      {children.map((page, index) => {
        return (
          <div className="carousel-page" key={index}>
            {page}
          </div>
        );
      })}
    </div>
  );
};

export default Carousel;
